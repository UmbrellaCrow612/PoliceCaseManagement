using Authorization.Core;
using Identity.API.Helpers;
using Identity.API.Services.Interfaces;
using Identity.API.Settings;
using Identity.Core.Models;
using Identity.Core.Repositorys;
using Identity.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UAParser;

namespace Identity.API.Services
{
    internal class AuthService(UserManager<ApplicationUser> userManager, DeviceManager deviceManager, IOptions<TimeWindows> options,  JwtBearerHelper jwtBearerHelper, IOptions<JwtBearerOptions> jwtBearerOptions, IUnitOfWork unitOfWork) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly DeviceManager _deviceManager = deviceManager;
        private readonly TimeWindows _timeWindows = options.Value;
        private readonly JwtBearerHelper _jwtBearerHelper = jwtBearerHelper;
        private readonly JwtBearerOptions _JwtBearerOptions = jwtBearerOptions.Value;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;


        private async Task<ApplicationUser?> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
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

            await _unitOfWork.Repository<LoginAttempt>().AddAsync(loginAttempt);

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, password);
            if (!isPasswordCorrect)
            {
                loginAttempt.FailureReason = "User credentials";

                await _unitOfWork.SaveChangesAsync();

                result.Errors.Add(new LoginError { Code = StatusCodes.Status401Unauthorized, Message = "Incorrect credentials" });
                return result;
            }

            if (user.LockoutEnabled is true && user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
            {
                loginAttempt.FailureReason = "User account locked.";

                await _unitOfWork.SaveChangesAsync();

                result.Errors.Add(new LoginError { Code = StatusCodes.Status401Unauthorized, Message = "Account locked" });
                return result;
            }

            if (!user.EmailConfirmed)
            {
                loginAttempt.FailureReason = "Email not confirmed.";

                await _unitOfWork.SaveChangesAsync();

                result.Errors.Add(new LoginError { Code = StatusCodes.Status401Unauthorized, Message = "Email not confirmed", RedirectUrl = IdentityApplicationRedirectUrls.EmailConfirmationUrl });
                return result;
            }

            if (!user.PhoneNumberConfirmed)
            {
                loginAttempt.FailureReason = "Phone not confirmed.";

                await _unitOfWork.SaveChangesAsync();

                result.Errors.Add(new LoginError { Code = StatusCodes.Status401Unauthorized, Message = "Phone number not confirmed", RedirectUrl = IdentityApplicationRedirectUrls.PhoneConfirmationUrl });
                return result;
            }

            var device = await _deviceManager.GetRequestingDevice(user.Id, deviceInfo.DeviceFingerPrint, deviceInfo.UserAgent);
            if(device is null || !device.Trusted())
            {
                loginAttempt.FailureReason = "Untrusted Device being used.";

                await _unitOfWork.SaveChangesAsync();

                result.Errors.Add(new LoginError { Code = StatusCodes.Status401Unauthorized, Message = "Untrsuted device being used", RedirectUrl = IdentityApplicationRedirectUrls.DeviceConfirmationUrl });
                return result;
            }

            loginAttempt.Status = LoginStatus.TwoFactorAuthenticationReached;
            await _unitOfWork.SaveChangesAsync();

            result.Succeeded = true;

            return result;
        }

        public async Task<TwoFactorEmailSentResult> SendTwoFactorEmailVerificationCodeAsync(string loginAttemptId)
        {
            var result = new TwoFactorEmailSentResult();

            var loginAttempt = await _unitOfWork.Repository<LoginAttempt>().FindByIdAsync(loginAttemptId);
            if (loginAttempt is null)
            {
                result.Errors.Add(new TwoFactorEmailSentResultError { Code = StatusCodes.Status404NotFound, Message = "Login attempt not found"});
                return result;
            }

            if (!loginAttempt.IsValid())
            {
                result.Errors.Add(new TwoFactorEmailSentResultError { Code = StatusCodes.Status401Unauthorized, Message = "Login attempt no longer valid" });
                return result;
            }

            var user = await _userManager.FindByIdAsync(loginAttempt.UserId);
            if (user is null)
            {
                result.Errors.Add(new TwoFactorEmailSentResultError { Code = StatusCodes.Status404NotFound, Message = "User not found" });
                return result;
            }

            if (!user.IsEmailConfirmed())
            {
                result.Errors.Add(new TwoFactorEmailSentResultError { Code = StatusCodes.Status401Unauthorized, Message = "User email not confirmed" });
                return result;
            }

            var validRecentTwoFactorEmailAttemptExists = await _unitOfWork.Repository<TwoFactorEmailAttempt>()
                .Query
                .Where(x => x.ExpiresAt > DateTime.UtcNow && x.LoginAttemptId == loginAttemptId && x.IsSuccessful == false)
                .OrderBy(x => x.CreatedAt)
                .AnyAsync();

            if (validRecentTwoFactorEmailAttemptExists)
            {
                result.Errors.Add(new TwoFactorEmailSentResultError { Code = StatusCodes.Status400BadRequest, Message = "Valid recent email attempt already issued for this login" });
                return result;
            }

            var twoFactorEmailAttempt = new TwoFactorEmailAttempt
            {
                Code = Guid.NewGuid().ToString()[..5],
                Email = user.Email!,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_timeWindows.TwoFactorEmailTime),
                LoginAttemptId = loginAttemptId,
            };

            await _unitOfWork.Repository<TwoFactorEmailAttempt>().AddAsync(twoFactorEmailAttempt);

            // use email service and send the code 

            await _unitOfWork.SaveChangesAsync();
            result.Succeeded = true;

            return result;
        }

        public async Task<TwoFactorSmsSentResult> SendTwoFactorSmsVerificationCodeAsync(string loginAttemptId)
        {
            var result = new TwoFactorSmsSentResult();

            var loginAttempt = await _unitOfWork.Repository<LoginAttempt>().FindByIdAsync(loginAttemptId);
            if(loginAttempt is null)
            {
                result.Errors.Add(new TwoFactorSmsSentResultError { Code = StatusCodes.Status404NotFound, Message = "LoginAsync loginAttempt not found" });
                return result;
            }

            if (!loginAttempt.IsValid())
            {
                result.Errors.Add(new TwoFactorSmsSentResultError { Code = StatusCodes.Status400BadRequest, Message = "LoginAsync loginAttempt no longer valid" });
                return result;
            }

            var user = await _userManager.FindByIdAsync(loginAttempt.UserId);
            if(user is null)
            {
                result.Errors.Add(new TwoFactorSmsSentResultError { Code = StatusCodes.Status404NotFound, Message = "User not found" });
                return result;
            }

            if (!user.IsPhoneNumberConfirmed())
            {
                result.Errors.Add(new TwoFactorSmsSentResultError { Code = StatusCodes.Status400BadRequest, Message = "User not phone number not confirmed" });
                return result;
            }

            var validRecentTwoFactorSmsAttemptExist = await _unitOfWork.Repository<TwoFactorSmsAttempt>()
                .Query
                .Where(x => x.ExpiresAt > DateTime.UtcNow && x.LoginAttemptId == loginAttemptId && x.IsSuccessful == false)
                .OrderBy(x => x.ExpiresAt)
                .AnyAsync();

            if (validRecentTwoFactorSmsAttemptExist)
            {
                result.Errors.Add(new TwoFactorSmsSentResultError { Code = StatusCodes.Status400BadRequest, Message = "There is a valid recent sms code issued already for this login attempt" });
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

            await _unitOfWork.Repository<TwoFactorSmsAttempt>().AddAsync(twoFactorSmsAttempt);

            await _unitOfWork.SaveChangesAsync();

            // use sms service and send it

            result.Succeeded = true;

            return result;
        }

        public async Task<(bool isValid, LoginAttempt? loginAttempt)> ValidateLoginAttemptAsync(string loginAttemptId)
        {
            var attempt = await _unitOfWork.Repository<LoginAttempt>().FindByIdAsync(loginAttemptId);
            if (attempt is null)
                return (false, null);

            return (attempt.IsValid(), attempt);
        }

        public async Task<TwoFactorEmailValidationResult> ValidateTwoFactorEmailCodeAsync(string loginAttemptId, string code, DeviceInfo deviceInfo)
        {
            var result = new TwoFactorEmailValidationResult();

            var (isValid, loginAttempt) = await ValidateLoginAttemptAsync(loginAttemptId);
            if(!isValid || loginAttempt is null)
            {
                result.Errors.Add(new TwoFactorEmailValidationError { Code = StatusCodes.Status401Unauthorized, Message = "Login attempt not found or is invalid" });
                return result;
            }

            var user = await GetUserByIdAsync(loginAttempt.UserId);
            if (user is null)
            {
                result.Errors.Add(new TwoFactorEmailValidationError { Code = StatusCodes.Status404NotFound, Message = "User not found" });
                return result;
            }

            var (isTrusted, userDevice) = await ValidateDeviceAsync(user.Id, deviceInfo);
            if(!isTrusted || userDevice is null)
            {
                result.Errors.Add(new TwoFactorEmailValidationError { Code = StatusCodes.Status401Unauthorized, Message = "User device not found or is not trusted" });
                return result;
            }

            var twoFactorEmailAttempt = await _unitOfWork.Repository<TwoFactorEmailAttempt>()
                .Query
                .Where(x => x.LoginAttemptId == loginAttemptId && x.Code == code)
                .FirstOrDefaultAsync();

            if (twoFactorEmailAttempt is null || !twoFactorEmailAttempt.IsValid())
            {
                result.Errors.Add(new TwoFactorEmailValidationError { Code = StatusCodes.Status401Unauthorized, Message = "Two factor code not found or is invalid" });
                return result;
            }

            twoFactorEmailAttempt.MarkUsed();
            _unitOfWork.Repository<TwoFactorEmailAttempt>().Update(twoFactorEmailAttempt);

            loginAttempt.MarkUsed();
            _unitOfWork.Repository<LoginAttempt>().Update(loginAttempt);

            var roles = await _userManager.GetRolesAsync(user);

            (string jwtBearerAcessToken, string jwtBearerAcessTokenId) = _jwtBearerHelper.GenerateBearerToken(user, roles);
            var refreshToken = _jwtBearerHelper.GenerateRefreshToken();

            result.Tokens.JwtBearerToken = jwtBearerAcessToken;
            result.Tokens.RefreshToken = refreshToken;

            var token = new Token
            {
                Id = jwtBearerAcessTokenId,
                RefreshToken = refreshToken,
                RefreshTokenExpiresAt = DateTime.UtcNow.AddMinutes(_JwtBearerOptions.RefreshTokenExpiriesInMinutes),
                UserDeviceId = userDevice.Id,
                UserId = user.Id
            };

            await _unitOfWork.Repository<Token>().AddAsync(token);

            user.LastLoginDeviceId = userDevice.Id;

            _unitOfWork.Repository<ApplicationUser>().Update(user);

            await _unitOfWork.SaveChangesAsync();

            result.Succeeded = true;

            return result;
        }

        public async Task<TwoFactorSmsValidationResult> ValidateTwoFactorSmsCodeAsync(string loginAttemptId, string code, DeviceInfo deviceInfo)
        {
            var result = new TwoFactorSmsValidationResult();

            var loginAttempt = await _unitOfWork.Repository<LoginAttempt>().FindByIdAsync(loginAttemptId);
            if (loginAttempt is null)
            {
                result.Errors.Add(new TwoFactorSmsValidationResultError { Code = StatusCodes.Status404NotFound, Message = "LoginAsync attempt not found" });
                return result;
            }

            if (!loginAttempt.IsValid())
            {
                result.Errors.Add(new TwoFactorSmsValidationResultError { Code = StatusCodes.Status400BadRequest, Message = "LoginAsync attempt no longer valid" });
                return result;
            }

            var user = await _userManager.FindByIdAsync(loginAttempt.UserId);
            if(user is null)
            {
                result.Errors.Add(new TwoFactorSmsValidationResultError { Code = StatusCodes.Status404NotFound, Message = "User not found" });
                return result;
            }

            var device = await _deviceManager.GetRequestingDevice(user.Id, deviceInfo.DeviceFingerPrint, deviceInfo.UserAgent);
            if(device is null || !device.Trusted())
            {
                result.Errors.Add(new TwoFactorSmsValidationResultError { Code = StatusCodes.Status401Unauthorized, Message = "Device not found or trsuted" });
                return result;
            }

            var smsAttempt = await _unitOfWork.Repository<TwoFactorSmsAttempt>().Query.Where(x => x.LoginAttemptId == loginAttemptId && x.Code == code).FirstOrDefaultAsync();
            if (smsAttempt is null)
            {
                result.Errors.Add(new TwoFactorSmsValidationResultError { Code = StatusCodes.Status404NotFound, Message = "Sms verifaction attempt not found for thus login attempt" });
                return result;
            }

            if (!smsAttempt.IsValid())
            {
                result.Errors.Add(new TwoFactorSmsValidationResultError { Code = StatusCodes.Status400BadRequest, Message = "Sms verifaction attempt not longer valid" });
                return result;
            }

            smsAttempt.MarkUsed();
            _unitOfWork.Repository<TwoFactorSmsAttempt>().Update(smsAttempt);

            loginAttempt.MarkUsed();
            _unitOfWork.Repository<LoginAttempt>().Update(loginAttempt);

            var roles = await _userManager.GetRolesAsync(user);

            (string jwtBearerAcessToken, string jwtBearerAcessTokenId) = _jwtBearerHelper.GenerateBearerToken(user, roles);
            var refreshToken = _jwtBearerHelper.GenerateRefreshToken();

            result.Tokens.JwtBearerToken = jwtBearerAcessToken;
            result.Tokens.RefreshToken = refreshToken;

            var token = new Token
            {
                Id = jwtBearerAcessTokenId,
                RefreshToken = refreshToken,
                RefreshTokenExpiresAt = DateTime.UtcNow.AddMinutes(_JwtBearerOptions.RefreshTokenExpiriesInMinutes),
                UserDeviceId = device.Id,
                UserId = user.Id
            };

            await _unitOfWork.Repository<Token>().AddAsync(token);

            user.LastLoginDeviceId = device.Id;

            _unitOfWork.Repository<ApplicationUser>().Update(user);

            await _unitOfWork.SaveChangesAsync();

            result.Succeeded = true;

            return result;
        }

        public async Task<(bool isTrusted, UserDevice? userDevice)> ValidateDeviceAsync(string userId, DeviceInfo info)
        {
            var device = await _deviceManager.GetRequestingDevice(userId, info.DeviceFingerPrint, info.UserAgent);
            if (device is null) return (false, null);

            if (!device.Trusted()) return (false, null);

            return (true, device);
        }
    }
}
