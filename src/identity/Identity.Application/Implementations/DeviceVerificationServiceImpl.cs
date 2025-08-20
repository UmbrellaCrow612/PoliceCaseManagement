using Identity.Application.Codes;
using Identity.Application.Settings;
using Identity.Core.Models;
using Identity.Core.Services;
using Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Results.Abstractions;

namespace Identity.Application.Implementations
{
    /// <summary>
    /// Business implementation of the contract <see cref="IDeviceVerificationService"/> - test this, as well when using it else where only use the <see cref="IDeviceVerificationService"/>
    /// interface not this class
    /// </summary>
    public class DeviceVerificationServiceImpl(
        IdentityApplicationDbContext dbContext, 
        ILogger<DeviceVerificationServiceImpl> logger, 
        ICodeGenerator codeGenerator,
        IOptions<TimeWindows> timeWindows
        ) : IDeviceVerificationService
    {
        private readonly IdentityApplicationDbContext _dbContext = dbContext;
        private readonly ILogger<DeviceVerificationServiceImpl> _logger = logger;
        private readonly ICodeGenerator _codeGenerator = codeGenerator;
        private readonly TimeWindows _timeWindows = timeWindows.Value;

        public async Task<IResult> SendVerification(ApplicationUser user, Device device)
        {
            var result = new Result();

            if (device.IsTrusted)
            {
                result.AddError(BusinessRuleCodes.DeviceAlreadyTrusted, "Device already trusted");
                return result;
            }

            // for this device, not used or expired
            var validAttemptExists = await _dbContext.DeviceVerifications.AnyAsync(x => x.UserId == user.Id && x.DeviceId == device.Id && x.UsedAt == null && x.ExpiresAt > DateTime.UtcNow);
            if (validAttemptExists)
            {
                result.AddError(BusinessRuleCodes.DeviceVerificationExists, "Valid attempt exists");
                return result;
            }

            var code = _codeGenerator.GenerateSixDigitCode();
            var newAttempt = new DeviceVerification
            {
                Code = code,
                DeviceId = device.Id,
                Email = user.Email!,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_timeWindows.DeviceChallengeTime),
                UserId = user.Id,
            };
            await _dbContext.DeviceVerifications.AddAsync(newAttempt);
            await _dbContext.SaveChangesAsync();

            #if DEBUG
            _logger.LogInformation("device verification code for device: {deviceId} is code: {code}", device.Id, code);
            #endif

            result.Succeeded = true;
            return result;
        }

        public async Task<IResult> Verify(Device device, string code)
        {
            var result = new Result();

            if (device.IsTrusted)
            {
                result.AddError(BusinessRuleCodes.DeviceAlreadyTrusted, "Device already trusted");
                return result;
            }

            var verification = await _dbContext.DeviceVerifications.Where(x => x.DeviceId == device.Id && x.Code == code).FirstOrDefaultAsync();
            if (verification is null || !verification.IsValid())
            {
                result.AddError(BusinessRuleCodes.DeviceVerificationInvalid, "Incorrect code");
                return result;
            }

            device.MarkTrusted();
            verification.MarkUsed();

            await _dbContext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }
    }
}
