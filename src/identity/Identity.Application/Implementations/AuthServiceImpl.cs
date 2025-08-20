using Identity.Application.Codes;
using Identity.Application.Settings;
using Identity.Core.Models;
using Identity.Core.Services;
using Identity.Core.ValueObjects;
using Identity.Infrastructure.Data;
using Microsoft.Extensions.Options;
using Results.Abstractions;

namespace Identity.Application.Implementations
{
    /// <summary>
    /// Business implementation of the contract <see cref="IAuthService"/> - test this, as well when using it else where only use the <see cref="IAuthService"/>
    /// interface not this class
    /// </summary>
    public class AuthServiceImpl(
        IUserService userService,
        IDeviceService deviceService, 
        IdentityApplicationDbContext dbContext,
        ITokenService tokenService,
        IOptions<TimeWindows> timeWindows
        ) : IAuthService
    {
        private readonly IDeviceService _deviceService = deviceService;
        private readonly IdentityApplicationDbContext _dbcontext = dbContext;
        private readonly ITokenService _tokenService = tokenService;
        private readonly TimeWindows _timeWindows = timeWindows.Value;
        private readonly IUserService _userService = userService;

        public async Task<LoginResult> LoginAsync(string email, string password, DeviceInfo deviceInfo)
        {
            var result = new LoginResult();

            var user = await _userService.FindByEmailAsync(email);

            if (user is null)
            {
                result.AddError(BusinessRuleCodes.IncorrectCredentials);
                return result;
            }

            var isPasswordCorrect = _userService.CheckPassword(user, password);
            if (!isPasswordCorrect)
            {
                result.AddError(BusinessRuleCodes.IncorrectCredentials);
                return result;
            }

            if (!user.EmailConfirmed)
            {
                result.AddError(BusinessRuleCodes.EmailNotConfirmed);
                return result;
            }

            if (!user.PhoneNumberConfirmed)
            {
                result.AddError(BusinessRuleCodes.PhoneNotConfirmed);
                return result;
            }

            var device = await _deviceService.GetDeviceAsync(user.Id, deviceInfo);
            if (device is null || !device.IsTrusted)
            {
                result.AddError(BusinessRuleCodes.Device);
                return result;
            }

            Login login = new()
            {
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_timeWindows.LoginLifetime),
                DeviceId = device.Id,
                Status = LoginStatus.TwoFactorAuthenticationReached
            };
            result.LoginId = login.Id;

            await _dbcontext.Logins.AddAsync(login);
            await _dbcontext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<RefreshResult> RefreshTokensAsync(string refreshToken, DeviceInfo deviceInfo)
        {
            var result = new RefreshResult();

            var token = await _tokenService.FindAsync(refreshToken);

            if (token is null)
            {
                result.AddError(BusinessRuleCodes.RefreshToken);
                return result;
            }

            var user = await _userService.FindByIdAsync(token.UserId);
            if (user is null)
            {
                result.AddError(BusinessRuleCodes.UserNotFound, "User dose not exist");
                return result;
            }

            var device = await _deviceService.GetDeviceAsync(user.Id, deviceInfo);
            if (device is null || !device.IsTrusted)
            {
                result.AddError(BusinessRuleCodes.Device);
                return result;
            }

            if (!token.IsValid(device.Id))
            {
                result.AddError(BusinessRuleCodes.RefreshToken);
                return result;
            }

            token.MarkUsed();

            var tokens = await _tokenService.IssueTokens(user, device);

            result.Tokens = tokens;

            await _dbcontext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<IResult> LogoutAsync(string userId)
        {
            var result = new Result();

            await _tokenService.RevokeTokensAsync(userId);

            result.Succeeded = true;
            return result;
        }
    }
}
