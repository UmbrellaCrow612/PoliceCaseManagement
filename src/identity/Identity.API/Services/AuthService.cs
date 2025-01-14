using Identity.API.Helpers;
using Identity.API.Services.Interfaces;
using Identity.API.Settings;
using Identity.Core.Models;
using Identity.Infrastructure.Data.Stores.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Services
{
    internal class AuthService(UserManager<ApplicationUser> userManager, ILoginAttemptStore loginAttemptStore, DeviceManager deviceManager) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILoginAttemptStore _loginAttemptStore = loginAttemptStore;
        private readonly DeviceManager _deviceManager = deviceManager;

        public async Task<LoginResult> Login(string email, string password, HttpRequest request)
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
                IpAddress = request.HttpContext.Connection.RemoteIpAddress?.ToString()
                                ?? request.Headers["X-Forwarded-For"].FirstOrDefault()
                                ?? "Unknown",
                UserAgent = request.Headers.UserAgent.ToString(),
                UserId = user.Id,
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

            var device = await _deviceManager.GetRequestingDevice(user.Id, request);
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
    }
}
