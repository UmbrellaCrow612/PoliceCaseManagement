using Identity.Application.Codes;
using Identity.Application.Settings;
using Identity.Core.Models;
using Identity.Core.Services;
using Identity.Core.ValueObjects;
using Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OtpNet;
using Results.Abstractions;
using System.Security;

namespace Identity.Application.Implementations
{
    /// <summary>
    /// Business implementation of the contract <see cref="IMfaService"/> - test this, as well when using it else where only use the <see cref="IMfaService"/>
    /// interface not this class
    /// </summary>
    public class MfaServiceImpl(
        IdentityApplicationDbContext dbContext, 
        IDeviceService deviceService, 
        ILogger<MfaServiceImpl> logger, 
        ICodeGenerator twoFactorCodeGenerator, 
        IOptions<TimeWindows> timeWindows, 
        IUserService userService,
        ITokenService tokenService
        ) : IMfaService
    {
        private readonly IdentityApplicationDbContext _dbContext = dbContext;
        private readonly IDeviceService _deviceService = deviceService;
        private readonly ILogger<MfaServiceImpl> _logger = logger;
        private readonly ICodeGenerator _twoFactorCodeGenerator = twoFactorCodeGenerator;
        private readonly TimeWindows _timeWindows = timeWindows.Value;
        private readonly IUserService _userService = userService;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<IResult> SendMfaSmsAsync(string loginId, DeviceInfo deviceInfo)
        {
            var result = new Result();

            var login = await _dbContext.Logins.FindAsync(loginId);
            if (login is null || !login.IsValid())
            {
                result.AddError(BusinessRuleCodes.Login);
                return result;
            }

            var user = await _userService.FindByIdAsync(login.UserId);
            if (user is null)
            {
                result.AddError(BusinessRuleCodes.UserNotFound);
                return result;
            }

            var device = await _deviceService.GetDeviceAsync(login.UserId, deviceInfo);
            if (device is null || !device.IsTrusted)
            {
                result.AddError(BusinessRuleCodes.Device);
                return result;
            }

            // Not expired, for this login and has not been used already
            var validRecentMfaSmsExists = await _dbContext.TwoFactorSms.AnyAsync(x => x.LoginId == login.Id && x.UsedAt == null && x.ExpiresAt > DateTime.UtcNow);
            if (validRecentMfaSmsExists)
            {
                result.AddError(BusinessRuleCodes.MFASmsExists);
                return result;
            }

            var code = _twoFactorCodeGenerator.GenerateSixDigitCode();
            var attempt = new TwoFactorSms
            {
                Code = code,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_timeWindows.TwoFactorSmsTime),
                LoginId = login.Id,
                PhoneNumber = user.PhoneNumber!,
                UserId = user.Id,
            };
            await _dbContext.TwoFactorSms.AddAsync(attempt);

            // send via twillio for phone number and code

            await _dbContext.SaveChangesAsync();

            #if DEBUG
            _logger.LogInformation("Two factor sms sent for userId: {userId} with code: {code}", user.Id, attempt.Code);
            #endif

            result.Succeeded = true;
            return result;
        }

        public async Task<VerifiedMfaResult> VerifyMfaSmsAsync(string loginId, string code, DeviceInfo deviceInfo)
        {
            var result = new VerifiedMfaResult();

            var login = await _dbContext.Logins.FindAsync(loginId);
            if (login is null || !login.IsValid())
            {
                result.AddError(BusinessRuleCodes.Login);
                return result;
            }

            var user = await _userService.FindByIdAsync(login.UserId);
            if (user is null)
            {
                result.AddError(BusinessRuleCodes.UserNotFound);
                return result;
            }

            var device = await _deviceService.GetDeviceAsync(login.UserId, deviceInfo);
            if (device is null || !device.IsTrusted)
            {
                result.AddError(BusinessRuleCodes.Device);
                return result;
            }

            var twoFactorSms = await _dbContext.TwoFactorSms.Where(x => x.LoginId == login.Id && x.Code == code).FirstOrDefaultAsync();
            if (twoFactorSms is null || !twoFactorSms.IsValid())
            {
                result.AddError(BusinessRuleCodes.MFAInvalid);
                return result;
            }

            login.MarkUsed();
            twoFactorSms.MarkUsed();

            var tokens = await _tokenService.IssueTokens(user, device);
            result.Tokens = tokens;

            await _dbContext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<VerifiedMfaResult> VerifyTotpAsync(string loginId, string code, DeviceInfo deviceInfo)
        {
            var result = new VerifiedMfaResult();

            var login = await _dbContext.Logins.FindAsync(loginId);
            if (login is null || !login.IsValid())
            {
                result.AddError(BusinessRuleCodes.Login);
                return result;
            }

            var user = await _userService.FindByIdAsync(login.UserId);
            if (user is null)
            {
                result.AddError(BusinessRuleCodes.UserNotFound);
                return result;
            }

            if (!user.TotpConfirmed || string.IsNullOrWhiteSpace(user.TotpSecret))
            {
                result.AddError(BusinessRuleCodes.TOTPReset, "TOTP not set on user or has not been confirmed");
                return result;
            }

            var device = await _deviceService.GetDeviceAsync(login.UserId, deviceInfo);
            if (device is null || !device.IsTrusted)
            {
                result.AddError(BusinessRuleCodes.Device);
                return result;
            }

            var totp = new Totp(Base32Encoding.ToBytes(user.TotpSecret));
            string computedTotpCode = totp.ComputeTotp();

            if (code != computedTotpCode)
            {
                result.AddError(BusinessRuleCodes.MFAInvalid, "Incorrect code");
                return result;
            }

            login.MarkUsed();

            var tokens = await _tokenService.IssueTokens(user, device);
            result.Tokens = tokens;

            await _dbContext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }
    }
}
