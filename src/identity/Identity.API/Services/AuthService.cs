using Authorization.Core;
using Identity.API.Helpers;
using Identity.API.Services.Interfaces;
using Identity.API.Settings;
using Identity.Core.Models;
using Identity.Infrastructure.Data;
using Identity.Infrastructure.Data.Stores.Interfaces;
using Identity.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Identity.API.Services
{
    internal class AuthService(UserManager<ApplicationUser> userManager, ILoginAttemptStore loginAttemptStore, DeviceManager deviceManager, IOptions<TimeWindows> options, ITwoFactorSmsAttemptStore twoFactorSmsAttemptStore, JwtBearerHelper jwtBearerHelper, IOptions<JwtBearerOptions> jwtBearerOptions, ITokenStore tokenStore) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILoginAttemptStore _loginAttemptStore = loginAttemptStore;
        private readonly DeviceManager _deviceManager = deviceManager;
        private readonly TimeWindows _timeWindows = options.Value;
        private readonly ITwoFactorSmsAttemptStore _twoFactorSmsAttemptStore = twoFactorSmsAttemptStore;
        private readonly JwtBearerHelper _jwtBearerHelper = jwtBearerHelper;
        private readonly JwtBearerOptions _JwtBearerOptions = jwtBearerOptions.Value;
        private readonly ITokenStore _tokenStore = tokenStore;

        public async Task<JwtBearerTokenResult> GenerateTokensAsync(string loginAttemptId, DeviceInfo deviceInfo)
        {
            var tokens = new Tokens();
            var result = new JwtBearerTokenResult() { Tokens = tokens };

            var loginAttempt = await _loginAttemptStore.FindAsync(loginAttemptId); // use of tracker api under the hood
            if (loginAttempt is null)
            {
                result.Errors.Add(new JwtBearerTokenError { Code = StatusCodes.Status404NotFound, Message = "Login attempt not found" });
                return result;
            }

            // we dont check is valid becuase we expect previous call validate sms to mark it as used to get to this point

            var user = await _userManager.FindByIdAsync(loginAttempt.UserId);
            if (user is null)
            {
                result.Errors.Add(new JwtBearerTokenError { Code = StatusCodes.Status404NotFound, Message = "User not found" });
                return result;
            }

            var device = await _deviceManager.GetRequestingDevice(user.Id, deviceInfo.DeviceFingerPrint, deviceInfo.UserAgent);  // use of tracker api under the hood
            if (device is null || !device.Trusted())
            {
                result.Errors.Add(new JwtBearerTokenError { Code = StatusCodes.Status401Unauthorized, Message = "Device not registred or trusted" });
                return result;
            }

            var roles = await _userManager.GetRolesAsync(user);

            (string jwtBearerAcessToken, string tokenId) = _jwtBearerHelper.GenerateBearerToken(user, roles);
            var refreshToken = _jwtBearerHelper.GenerateRefreshToken();

            tokens.JwtBearerToken = jwtBearerAcessToken;
            tokens.RefreshToken = refreshToken;

            var token = new Token 
            { 
                Id = tokenId, 
                RefreshToken = refreshToken, 
                RefreshTokenExpiresAt = DateTime.UtcNow.AddMinutes(_JwtBearerOptions.RefreshTokenExpiriesInMinutes), 
                UserDeviceId = device.Id,
                UserId = user.Id 
            };

            await _tokenStore.SetToken(token);

            user.LastLoginDeviceId = device.Id;

            var res2 = await _userManager.UpdateAsync(user); // calls db save changes
            if (!res2.Succeeded)
            {
                foreach (var err in res2.Errors)
                {
                    result.Errors.Add(new JwtBearerTokenError { Code = StatusCodes.Status500InternalServerError, Message = err.Description });
                }
            }

            result.Succeeded = true;

            return result;
        }

        public async Task<LoginResult> LoginAsync(string email, string password, DeviceInfo deviceInfo)
        {
            var result = new LoginResult();
            var user = await _userManager.FindByEmailAsync(email);

            if(user is null)
            {
                result.Errors.Add(new LoginError { Code = StatusCodes.Status401Unauthorized, Message = "User not found" });
                return result;
            }

            LoginAttempt loginAttempt = new()
            {
                IpAddress = deviceInfo.IpAddress,
                UserAgent = deviceInfo.UserAgent,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_timeWindows.LoginLifetime),
            };
            result.LoginAttemptId = loginAttempt.Id;

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, password);
            if (!isPasswordCorrect)
            {
                loginAttempt.FailureReason = "User credentials";
                await _loginAttemptStore.StoreLoginAttemptAsync(loginAttempt);

                result.Errors.Add(new LoginError { Code = StatusCodes.Status401Unauthorized, Message = "Incorrect credentials" });
                return result;
            }

            if (user.LockoutEnabled is true && user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
            {
                loginAttempt.FailureReason = "User account locked.";
                await _loginAttemptStore.StoreLoginAttemptAsync(loginAttempt);

                result.Errors.Add(new LoginError { Code = StatusCodes.Status401Unauthorized, Message = "Account locked" });
                return result;
            }

            if (!user.EmailConfirmed)
            {
                loginAttempt.FailureReason = "Email not confirmed.";
                await _loginAttemptStore.StoreLoginAttemptAsync(loginAttempt);

                result.Errors.Add(new LoginError { Code = StatusCodes.Status401Unauthorized, Message = "Email not confirmed", RedirectUrl = IdentityApplicationRedirectUrls.EmailConfirmationUrl });
                return result;
            }

            if (!user.PhoneNumberConfirmed)
            {
                loginAttempt.FailureReason = "Phone not confirmed.";
                await _loginAttemptStore.StoreLoginAttemptAsync(loginAttempt);

                result.Errors.Add(new LoginError { Code = StatusCodes.Status401Unauthorized, Message = "Phone number not confirmed", RedirectUrl = IdentityApplicationRedirectUrls.PhoneConfirmationUrl });
                return result;
            }

            var device = await _deviceManager.GetRequestingDevice(user.Id, deviceInfo.DeviceFingerPrint, deviceInfo.UserAgent);
            if(device is null || !device.Trusted())
            {
                loginAttempt.FailureReason = "Untrusted Device being used.";
                await _loginAttemptStore.StoreLoginAttemptAsync(loginAttempt);

                result.Errors.Add(new LoginError { Code = StatusCodes.Status401Unauthorized, Message = "Untrsuted device being used", RedirectUrl = IdentityApplicationRedirectUrls.DeviceConfirmationUrl });
                return result;
            }

            loginAttempt.Status = LoginStatus.TwoFactorAuthenticationReached;
            await _loginAttemptStore.StoreLoginAttemptAsync(loginAttempt);

            result.Succeeded = true;

            return result;
        }

        public async Task<TwoFactorSmsVerificationResult> SendTwoFactorSmsVerificationCodeAsync(string loginAttemptId)
        {
            var result = new TwoFactorSmsVerificationResult();

            var loginAttempt = await _loginAttemptStore.FindAsync(loginAttemptId);
            if(loginAttempt is null)
            {
                result.Errors.Add(new TwoFactorSmsVerificationError { Code = StatusCodes.Status404NotFound, Message = "LoginAsync loginAttempt not found" });
                return result;
            }

            if (!loginAttempt.IsValid())
            {
                result.Errors.Add(new TwoFactorSmsVerificationError { Code = StatusCodes.Status400BadRequest, Message = "LoginAsync loginAttempt no longer valid" });
                return result;
            }

            var user = await _userManager.FindByIdAsync(loginAttempt.UserId);
            if(user is null)
            {
                result.Errors.Add(new TwoFactorSmsVerificationError { Code = StatusCodes.Status404NotFound, Message = "User not found" });
                return result;
            }

            if (!user.IsPhoneNumberConfirmed())
            {
                result.Errors.Add(new TwoFactorSmsVerificationError { Code = StatusCodes.Status400BadRequest, Message = "User not phone number not confirmed" });
                return result;
            }

            var validRecentTwoFactorSmsAttemptExist = await _twoFactorSmsAttemptStore.TwoFactorSmsAttempts
                .Where(x => x.ExpiresAt > DateTime.UtcNow && x.LoginAttemptId == loginAttemptId && x.IsSuccessful == false)
                .OrderBy(x => x.ExpiresAt)
                .AnyAsync();

            if (validRecentTwoFactorSmsAttemptExist)
            {
                result.Errors.Add(new TwoFactorSmsVerificationError { Code = StatusCodes.Status400BadRequest, Message = "There is a valid recent sms code issued already for this login attempt" });
                return result;
            }

            var twoFactorSmsAttempt = new TwoFactorSmsAttempt
            {
                Code = Guid.NewGuid().ToString()[..5],
                LoginAttemptId = loginAttemptId,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_timeWindows.TwoFactorSmsTime),
                PhoneNumber = user.PhoneNumber!,
                UserId = user.Id,
            };

            await _twoFactorSmsAttemptStore.AddAsync(twoFactorSmsAttempt);

            // use sms service and send it

            result.Succeeded = true;

            return result;
        }

        public async Task<TwoFactorSmsVerificationResult> ValidateTwoFactorSmsCodeAsync(string loginAttemptId, string code, DeviceInfo deviceInfo)
        {
            var result = new TwoFactorSmsVerificationResult();

            var loginAttempt = await _loginAttemptStore.FindAsync(loginAttemptId);
            if (loginAttempt is null)
            {
                result.Errors.Add(new TwoFactorSmsVerificationError { Code = StatusCodes.Status404NotFound, Message = "LoginAsync attempt not found" });
                return result;
            }

            if (!loginAttempt.IsValid())
            {
                result.Errors.Add(new TwoFactorSmsVerificationError { Code = StatusCodes.Status400BadRequest, Message = "LoginAsync attempt no longer valid" });
                return result;
            }

            var user = await _userManager.FindByIdAsync(loginAttempt.UserId);
            if(user is null)
            {
                result.Errors.Add(new TwoFactorSmsVerificationError { Code = StatusCodes.Status404NotFound, Message = "User not found" });
                return result;
            }

            var device = await _deviceManager.GetRequestingDevice(user.Id, deviceInfo.DeviceFingerPrint, deviceInfo.UserAgent);
            if(device is null || !device.Trusted())
            {
                result.Errors.Add(new TwoFactorSmsVerificationError { Code = StatusCodes.Status401Unauthorized, Message = "Device not found or trsuted" });
                return result;
            }

            var smsAttempt = await _twoFactorSmsAttemptStore.FindAsync(loginAttemptId, code);
            if (smsAttempt is null)
            {
                result.Errors.Add(new TwoFactorSmsVerificationError { Code = StatusCodes.Status404NotFound, Message = "Sms verifaction attempt not found for thus login attempt" });
                return result;
            }

            if (!smsAttempt.IsValid())
            {
                result.Errors.Add(new TwoFactorSmsVerificationError { Code = StatusCodes.Status400BadRequest, Message = "Sms verifaction attempt not longer valid" });
                return result;
            }

            smsAttempt.MarkUsed();
            _twoFactorSmsAttemptStore.SetToUpdateAttempt(smsAttempt);

            loginAttempt.MarkUsed();
            _loginAttemptStore.SetToUpdateAttempt(loginAttempt);

            result.Succeeded = true;

            return result;
        }
    }
}
