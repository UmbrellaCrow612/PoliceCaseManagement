using System.Data;
using Identity.Application.Codes;
using Identity.Application.Helpers;
using Identity.Application.Settings;
using Identity.Core.Models;
using Identity.Core.Services;
using Identity.Core.ValueObjects;
using Identity.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Identity.Application.Implementations
{
    /// <summary>
    /// Business implementation of the contract <see cref="IAuthService"/> - test this, as well when using it else where only use the <see cref="IAuthService"/>
    /// interface not this class
    /// </summary>
    public class AuthServiceImpl(
        UserManager<ApplicationUser> userManager
        ,IDeviceService deviceService, 
        IdentityApplicationDbContext dbContext,
        ITokenService tokenService,
        IOptions<TimeWindows> timeWindows
        ) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IDeviceService _deviceService = deviceService;
        private readonly IdentityApplicationDbContext _dbcontext = dbContext;
        private readonly ITokenService _tokenService = tokenService;
        private readonly TimeWindows _timeWindows = timeWindows.Value;

        public async Task<LoginResult> LoginAsync(string email, string password, DeviceInfo deviceInfo)
        {
            var result = new LoginResult();

            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                result.AddError(BusinessRuleCodes.IncorrectCredentials);
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
                DeviceId = device.Id
            };

            result.LoginId = login.Id;

            await _dbcontext.Logins.AddAsync(login);

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, password);
            if (!isPasswordCorrect)
            {
                await _dbcontext.SaveChangesAsync();
                result.AddError(BusinessRuleCodes.IncorrectCredentials);

                return result;
            }

            if (!user.EmailConfirmed)
            {
                await _dbcontext.SaveChangesAsync();
                result.AddError(BusinessRuleCodes.EmailNotConfirmed);

                return result;
            }

            if (!user.PhoneNumberConfirmed)
            {
                await _dbcontext.SaveChangesAsync();
                result.AddError(BusinessRuleCodes.PhoneNotConfirmed);

                return result;
            }
   

            login.Status = LoginStatus.TwoFactorAuthenticationReached;
            await _dbcontext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<RefreshResult> RefreshTokensAsync(string refreshToken, DeviceInfo deviceInfo)
        {
            var result = new RefreshResult();

            var token = await _dbcontext.Tokens.FindAsync(refreshToken);

            if (token is null)
            {
                result.AddError(BusinessRuleCodes.RefreshToken);
                return result;
            }

            var user = await _userManager.FindByIdAsync(token.UserId);
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
            _dbcontext.Tokens.Update(token);

            var tokens = await _tokenService.IssueTokens(user, device);

            result.Tokens = tokens;

            await _dbcontext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<AuthResult> LogoutAsync(string userId)
        {
            var result = new AuthResult();

             await _dbcontext.Tokens
                .Where(x => x.RevokedAt == null && x.UserId == userId && x.ExpiresAt > DateTime.UtcNow && x.UsedAt == null)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(t => t.RevokedAt, DateTime.UtcNow)
                );

            result.Succeeded = true;
            return result;
        }
    }
}
