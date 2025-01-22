using Authorization.Core;
using Identity.API.Helpers;
using Identity.API.Services.Interfaces;
using Identity.API.Settings;
using Identity.Core.Models;
using Identity.Core.Repositorys;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OtpNet;
using System.Data;
using System.Security.Cryptography;

namespace Identity.API.Services
{
    internal class AuthService(UserManager<ApplicationUser> userManager, DeviceManager deviceManager, IOptions<TimeWindows> options,  JwtBearerHelper jwtBearerHelper, IOptions<JwtBearerOptions> jwtBearerOptions, IUnitOfWork unitOfWork, IOptions<IdentityApplicationRedirectUrls> redirectOptions) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly DeviceManager _deviceManager = deviceManager;
        private readonly TimeWindows _timeWindows = options.Value;
        private readonly IdentityApplicationRedirectUrls redirectUrls = redirectOptions.Value;
        private readonly JwtBearerHelper _jwtBearerHelper = jwtBearerHelper;
        private readonly JwtBearerOptions _JwtBearerOptions = jwtBearerOptions.Value;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private async Task<Tokens> GenerateAndStoreTokens(ApplicationUser user, UserDevice device)
        {
            var roles = await _userManager.GetRolesAsync(user);

            (string jwtBearerAcessToken, string jwtBearerAcessTokenId) = _jwtBearerHelper.GenerateBearerToken(user, roles);
            var refreshToken = _jwtBearerHelper.GenerateRefreshToken();

            var token = new Token
            {
                Id = jwtBearerAcessTokenId,
                RefreshToken = refreshToken,
                RefreshTokenExpiresAt = DateTime.UtcNow.AddMinutes(_JwtBearerOptions.RefreshTokenExpiriesInMinutes),
                UserDeviceId = device.Id,
                UserId = user.Id
            };

            await _unitOfWork.Repository<Token>().AddAsync(token);

            return new Tokens { JwtBearerToken = jwtBearerAcessToken, RefreshToken = refreshToken };
        }

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
                result.AddError(StatusCodes.Status404NotFound, "user not found");
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

                result.Errors.Add(new LoginError { Code = StatusCodes.Status401Unauthorized, Message = "Email not confirmed", RedirectUrl = redirectUrls.EmailConfirmationUrl });
                return result;
            }

            if (!user.PhoneNumberConfirmed)
            {
                loginAttempt.FailureReason = "Phone not confirmed.";

                await _unitOfWork.SaveChangesAsync();

                result.Errors.Add(new LoginError { Code = StatusCodes.Status401Unauthorized, Message = "Phone number not confirmed", RedirectUrl = redirectUrls.PhoneConfirmationUrl });
                return result;
            }

            var device = await _deviceManager.GetRequestingDevice(user.Id, deviceInfo.DeviceFingerPrint, deviceInfo.UserAgent);
            if(device is null || !device.Trusted())
            {
                loginAttempt.FailureReason = "Untrusted Device being used.";

                await _unitOfWork.SaveChangesAsync();

                result.Errors.Add(new LoginError { Code = StatusCodes.Status401Unauthorized, Message = "Untrsuted device being used", RedirectUrl = redirectUrls.DeviceConfirmationUrl });
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

            var tokens = await GenerateAndStoreTokens(user, userDevice);
            result.Tokens = tokens;

            user.SetLastUsedDevice(userDevice.Id);

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

        public async Task<RefreshTokenResult> RefreshTokens(string userId, string tokenId, string refreshToken, DeviceInfo deviceInfo)
        {
            var result = new RefreshTokenResult();

            var user = await GetUserByIdAsync(userId);
            if (user is null)
            {
                return result;
            }

            var device = await _deviceManager.GetRequestingDevice(user.Id, deviceInfo.DeviceFingerPrint, deviceInfo.UserAgent);
            if (device is null) return result;

            var storedToken = await _unitOfWork.Repository<Token>().FindByIdAsync(tokenId);
            if (storedToken is null || !storedToken.IsValid(refreshToken, device.Id)) return result;

            var roles = await _userManager.GetRolesAsync(user);

            var (jwtBearerToken, jwtBearerAcessTokenId) = _jwtBearerHelper.GenerateBearerToken(user, roles);

            var newToken = new Token
            {
                Id = jwtBearerAcessTokenId,
                RefreshToken = storedToken.RefreshToken,
                RefreshTokenExpiresAt = storedToken.RefreshTokenExpiresAt, // bind to previous refresh token and time
                UserDeviceId = device.Id,
                UserId = user.Id,
            };
           result.Tokens.JwtBearerToken = jwtBearerToken;

            await _unitOfWork.Repository<Token>().AddAsync(newToken);
            await _unitOfWork.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<LogoutResult> LogoutAsync(string userId)
        {
            var result = new LogoutResult();

            var user = GetUserByIdAsync(userId);
            if (user is null) return result;

            var validTokens = await _unitOfWork.Repository<Token>()
                .Query
                .Where(x => x.IsRevoked == false && x.UserId == userId && x.RefreshTokenExpiresAt > DateTime.UtcNow)
                .ToArrayAsync();

            foreach ( var token in validTokens)
            {
                token.Revoke();
            }

            _unitOfWork.Repository<Token>().UpdateRange(validTokens);
            await _unitOfWork.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<SendMagicLinkResult> SendMagicLink(string email, DeviceInfo device)
        {
            var result = new SendMagicLinkResult();

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return result;

            if (!user.IsMagicLinkAuthEnabled()) return result;
            if (!user.IsEmailConfirmed()) return result;

            var (isTrusted, userDevice) = await ValidateDeviceAsync(user.Id, device);
            if(!isTrusted || userDevice is null) return result;

            var validRecentAttemptExists = await _unitOfWork.Repository<MagicLinkAttempt>()
                .Query
                .Where(x => x.ExpiresAt > DateTime.UtcNow && x.UserId == user.Id && x.IsUsed == false)
                .AnyAsync();

            if (validRecentAttemptExists) return result;

            var magicLink = new MagicLinkAttempt()
            {
                ExpiresAt = DateTime.UtcNow.AddMinutes(1),
                UserId = user.Id,
            };

            await _unitOfWork.Repository<MagicLinkAttempt>().AddAsync(magicLink);
            await _unitOfWork.SaveChangesAsync();

            // send email using email service domain/magic-link?code=magicLink.id

            result.Succeeded = true;
            return result;
        }

        public async Task<ValidateMagicLinkResult> ValidateMagicLink(string code, DeviceInfo device)
        {
            var result = new ValidateMagicLinkResult();

            var magicLinkAttempt = await _unitOfWork.Repository<MagicLinkAttempt>().FindByIdAsync(code);
            if (magicLinkAttempt is null || !magicLinkAttempt.IsValid()) return result;

            var user = await GetUserByIdAsync(magicLinkAttempt.UserId);
            if (user is null) return result;

            var (isTrusted, userDevice) = await ValidateDeviceAsync(user.Id, device);
            if (!isTrusted || userDevice is null) return result;

            magicLinkAttempt.MarkUsed();
            _unitOfWork.Repository<MagicLinkAttempt>().Update(magicLinkAttempt);

            var tokens = await GenerateAndStoreTokens(user, userDevice);
            result.Tokens = tokens;

            await _unitOfWork.SaveChangesAsync();
            result.Succeeded = true;

            return result;
        }

        public async Task<SendOTPResult> SendOTP(OTPMethod method, OTPCreds creds, DeviceInfo device)
        {
            var result = new SendOTPResult();

            if (string.IsNullOrWhiteSpace(creds.Email) && string.IsNullOrWhiteSpace(creds.PhoneNumber)) return result;

            if (method == OTPMethod.Email)
            {
                if (string.IsNullOrWhiteSpace(creds.Email)) return result;

                var user = await _userManager.FindByEmailAsync(creds.Email);
                if(user is null || !user.IsEmailConfirmed() || !user.IsOTPAuthEnabled()) return result;

                var validRecentOTPEmailExists = await _unitOfWork.Repository<OTPAttempt>()
                    .Query
                    .Where(x => x.ExpiresAt > DateTime.UtcNow && x.UserId == user.Id && x.IsUsed == false && x.Method == OTPMethod.Email)
                    .AnyAsync();

                if (validRecentOTPEmailExists) return result;

                var newAttempt = new OTPAttempt
                {
                    Code = Guid.NewGuid().ToString()[..5], // could be more secure not bothered atm
                    ExpiresAt = DateTime.UtcNow.AddMinutes(1),
                    UserId = user.Id,
                    Method = OTPMethod.Email,
                };

                await _unitOfWork.Repository<OTPAttempt>().AddAsync(newAttempt);
                await _unitOfWork.SaveChangesAsync();

                // use email service and send OTP code
                result.Succeeded = true;

                return result;
            }

            if (method == OTPMethod.Sms)
            {
                if (string.IsNullOrWhiteSpace(creds.PhoneNumber)) return result;

                var user = await _userManager.Users.Where(x => x.PhoneNumber == creds.PhoneNumber).FirstOrDefaultAsync();
                if (user is null || !user.IsPhoneNumberConfirmed() || !user.IsOTPAuthEnabled()) return result;

                var validRecentOTPSmsExists = await _unitOfWork.Repository<OTPAttempt>()
                  .Query
                  .Where(x => x.ExpiresAt > DateTime.UtcNow && x.UserId == user.Id && x.IsUsed == false && x.Method == OTPMethod.Sms)
                  .AnyAsync();

                if (validRecentOTPSmsExists) return result;

                var newAttempt = new OTPAttempt
                {
                    Code = Guid.NewGuid().ToString()[..5],
                    ExpiresAt = DateTime.UtcNow.AddMinutes(1),
                    UserId = user.Id,
                    Method = OTPMethod.Sms,
                };

                await _unitOfWork.Repository<OTPAttempt>().AddAsync(newAttempt);
                await _unitOfWork.SaveChangesAsync();

                // use sms service and send OTP Code
                result.Succeeded = true;

                return result;
            }

            return result;
        }

        public async Task<ValidateOTPResult> ValidateOTP(OTPMethod method, OTPCreds creds, string code, DeviceInfo device)
        {
            var result = new ValidateOTPResult();

            if (string.IsNullOrWhiteSpace(creds.Email) && string.IsNullOrWhiteSpace(creds.PhoneNumber)) return result;

            if (method == OTPMethod.Email)
            {
                if (string.IsNullOrWhiteSpace(creds.Email)) return result;

                var user = await _userManager.FindByEmailAsync(creds.Email);
                if (user is null || !user.IsEmailConfirmed() || !user.IsOTPAuthEnabled()) return result;

                var (isTrusted, userDevice) = await ValidateDeviceAsync(user.Id, device);
                if (!isTrusted || userDevice is null) return result;

                var otpEmailAttempt = await _unitOfWork.Repository<OTPAttempt>()
                    .Query
                    .Where(x => x.UserId == user.Id && x.Code == code)
                    .FirstOrDefaultAsync();

                if (otpEmailAttempt is null || !otpEmailAttempt.IsValid()) return result;

                otpEmailAttempt.MarkUsed();
                _unitOfWork.Repository<OTPAttempt>().Update(otpEmailAttempt);

                var tokens = await GenerateAndStoreTokens(user, userDevice);
                result.Tokens = tokens;

                await _unitOfWork.SaveChangesAsync();
                result.Succeeded = true;

                return result;
            }

            // do sms later

            return result;
        }

        public async Task<SetUpTOTPResult> SetUpTOTP(string userId, DeviceInfo deviceInfo)
        {
            var result = new SetUpTOTPResult();

            var user = await GetUserByIdAsync(userId);
            if (user is null || !user.IsTOTPAuthEnabled()) return result;

            var (isTrusted, userDevice) = await ValidateDeviceAsync(user.Id, deviceInfo);
            if (!isTrusted || userDevice is null) return result;

            var totpExists = await _unitOfWork.Repository<TimeBasedOneTimePassCode>()
                .Query
                .Where(x => x.UserId == user.Id)
                .AnyAsync();

            if (totpExists) return result;

            byte[] secretBytes = new byte[20];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(secretBytes);
            }

            string base32Secret = Base32Encoding.ToString(secretBytes);

            byte[] secretAsQrCodebytes = QRCodeHandler.GenerateQRCodeBytes(
                        base32Secret,
                        user.Email!,  
                        "YourAppName"  
                        );
            result.TotpSecretQrCodeBytes = secretAsQrCodebytes;

            var totp = new TimeBasedOneTimePassCode
            {
                UserId = user.Id,
                Secret = base32Secret  
            };

            await _unitOfWork.Repository<TimeBasedOneTimePassCode>().AddAsync(totp);
            await _unitOfWork.SaveChangesAsync();

            result.Succeeded = true;

            return result;
        }

        public async Task<ValidateTOTPResult> ValidateTOTP(string email, string code, DeviceInfo deviceInfo)
        {
            var result = new ValidateTOTPResult();

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null || !user.IsTOTPAuthEnabled()) return result;

            var (isTrusted, userDevice) = await ValidateDeviceAsync(user.Id, deviceInfo);
            if (!isTrusted || userDevice is null) return result;

            var storedTotp = await _unitOfWork.Repository<TimeBasedOneTimePassCode>()
                .Query
                .Where(x => x.UserId == user.Id).FirstOrDefaultAsync();

            if (storedTotp is null) return result;

            var bytes = Base32Encoding.ToBytes(storedTotp.Secret);
            var totp = new Totp(bytes);
            var trueCode = totp.ComputeTotp(); 

            if (!string.Equals(trueCode, code)) return result;

            var tokens = await GenerateAndStoreTokens(user, userDevice);
            result.Tokens = tokens;

            await _unitOfWork.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<SendResetPasswordResult> SendResetPassword(string email)
        {
            var result = new SendResetPasswordResult();

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                result.Succeeded = true; // dont reveal if user exists
                return result;
            }

            var validRecentResetPasswordAttemptExists = await _unitOfWork.Repository<PasswordResetAttempt>()
                .Query
                .Where(x => x.ExpiresAt > DateTime.UtcNow && x.UserId == user.Id && x.IsUsed == false)
                .AnyAsync();

            if (validRecentResetPasswordAttemptExists) return result;

            var newAttempt = new PasswordResetAttempt
            {
                Code = Guid.NewGuid().ToString(), // Is unique
                ExpiresAt = DateTime.UtcNow.AddMinutes(_timeWindows.ResetPasswordTime),
                UserId = user.Id,
            };

            await _unitOfWork.Repository<PasswordResetAttempt>().AddAsync(newAttempt);
            await _unitOfWork.SaveChangesAsync();

            // send email using email service domain/reset-password?code=newAttempt.code

            result.Succeeded = true;

            return result;
        }

        public async Task<ValidateResetPasswordResult> ValidateResetPassword(string code, string newPassword)
        {
            var result = new ValidateResetPasswordResult();

            var attempt = await _unitOfWork.Repository<PasswordResetAttempt>()
                .Query
                .FirstOrDefaultAsync(x => x.Code == code);

            if (attempt is null || !attempt.IsValid()) return result;

            var user = await GetUserByIdAsync(attempt.UserId);
            if (user is null) return result;

            attempt.MarkUsed();

            _unitOfWork.Repository<PasswordResetAttempt>().Update(attempt);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var changePasswordResult = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!changePasswordResult.Succeeded) return result;

            result.Succeeded = true;
            return result;
        }

        public async Task<SendConfirmationEmailResult> SendConfirmationEmail(string email)
        {
            var result = new SendConfirmationEmailResult();

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null || user.IsEmailConfirmed()) return result;

            var validRecentEmailConfirmationAttemptExists = await _unitOfWork.Repository<EmailVerificationAttempt>()
                .Query
                .Where(x => x.ExpiresAt > DateTime.UtcNow && x.UserId == user.Id && x.IsUsed == false)
                .AnyAsync();

            if (validRecentEmailConfirmationAttemptExists) return result;

            var newAttempt = new EmailVerificationAttempt
            {
                Code = Guid.NewGuid().ToString(),
                Email = user.Email!,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_timeWindows.EmailConfirmationTime),
                UserId = user.Id,
            };

            // send email using email service domain/confirm-email?code=newAttempt.code

            await _unitOfWork.Repository<EmailVerificationAttempt>().AddAsync(newAttempt);
            await _unitOfWork.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<ConfirmEmailResult> ConfirmEmail(string email, string code)
        {
            var result = new ConfirmEmailResult();

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null || user.IsEmailConfirmed()) return result;

            var attempt = await _unitOfWork.Repository<EmailVerificationAttempt>()
                .Query
                .FirstOrDefaultAsync(x => x.UserId == user.Id && x.Code == code);

            if (attempt is null || !attempt.IsValid()) return result;
            user.EmailConfirmed = true;

            _unitOfWork.Repository<ApplicationUser>().Update(user);
            attempt.MarkUsed();
            _unitOfWork.Repository<EmailVerificationAttempt>().Update(attempt);

            await _unitOfWork.SaveChangesAsync();
            result.Succeeded = true;

            return result;
        }

        public async Task<SendUserDeviceChallengeResult> SendUserDeviceChallenge(string email, DeviceInfo info)
        {
            var result = new SendUserDeviceChallengeResult();

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return result;

            var deviceId = _deviceManager.GenerateDeviceId(user.Id, info.DeviceFingerPrint, info.UserAgent);
            var deviceExists = await _unitOfWork.Repository<UserDevice>().Query.Where(x => x.Id == deviceId).AnyAsync();
            if (!deviceExists)
            {
                var deviceToAdd = new UserDevice
                {
                    DeviceName = info.UserAgent,
                    Id = deviceId,
                    UserId = user.Id,
                };
                await _unitOfWork.Repository<UserDevice>().AddAsync(deviceToAdd);
            }

            var device = await _unitOfWork.Repository<UserDevice>().FindByIdAsync(deviceId);
            if (device is null || device.IsTrusted) return result;

            var validRecentAttemptExists = await _unitOfWork.Repository<UserDeviceChallengeAttempt>()
                .Query
                .Where(x => x.ExpiresAt > DateTime.UtcNow && x.UserId == user.Id && x.UserDeviceId == device.Id && x.IsUsed == false)
                .AnyAsync();

            if (validRecentAttemptExists) return result;

            var attempt = new UserDeviceChallengeAttempt
            {
                Code = Guid.NewGuid().ToString(),
                Email = user.Email!,
                UserDeviceId = device.Id,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_timeWindows.DeviceChallengeTime),   
            };
            await _unitOfWork.Repository<UserDeviceChallengeAttempt>().AddAsync(attempt);
            await _unitOfWork.SaveChangesAsync();

            // send email using email service domain/device-challenge?code=attempt.code

            result.Succeeded = true;
            return result;
        }

        public async Task<ValidateUserDeviceChallengeResult> ValidateUserDeviceChallenge(string email, string code)
        {
            var result = new ValidateUserDeviceChallengeResult();

            var attempt = await _unitOfWork.Repository<UserDeviceChallengeAttempt>()
                .Query
                .FirstOrDefaultAsync(x => x.Code == code && x.Email == email);
            if (attempt is null || !attempt.IsValid()) return result;

            var device = await _unitOfWork.Repository<UserDevice>().FindByIdAsync(attempt.UserDeviceId);
            if (device is null || device.IsTrusted) return result;

            attempt.MarkUsed();
            _unitOfWork.Repository<UserDeviceChallengeAttempt>().Update(attempt);
            device.IsTrusted = true;
            _unitOfWork.Repository<UserDevice>().Update(device);

            await _unitOfWork.SaveChangesAsync();
            result.Succeeded = true;
            return result;
        }

        public async Task<SendPhoneConfirmationResult> SendPhoneConfirmation(string phoneNumber)
        {
            var result = new SendPhoneConfirmationResult();

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
            if (user is null || user.PhoneNumberConfirmed) return result;

            var validRecentPhoneConfirmationAttemptExists = await _unitOfWork.Repository<PhoneConfirmationAttempt>()
                .Query
                .Where(x => x.ExpiresAt > DateTime.UtcNow && x.UserId == user.Id && x.IsUsed == false)
                .AnyAsync();

            if (validRecentPhoneConfirmationAttemptExists) return result;

            var newAttempt = new PhoneConfirmationAttempt
            {
                Code = Guid.NewGuid().ToString()[..5],
                ExpiresAt = DateTime.UtcNow.AddMinutes(_timeWindows.PhoneConfirmationTime),
                PhoneNumber = user.PhoneNumber!,
                UserId = user.Id,
            };
            await _unitOfWork.Repository<PhoneConfirmationAttempt>().AddAsync(newAttempt);
            await _unitOfWork.SaveChangesAsync();

            // send sms using sms service
            result.Succeeded = true;
            return result;
        }

        public async Task<ConfirmPhoneNumberResult> ConfirmPhoneNumber(string phoneNumber, string code)
        {
            var result = new ConfirmPhoneNumberResult();

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
            if (user is null || user.PhoneNumberConfirmed) return result;

            var attempt = await _unitOfWork.Repository<PhoneConfirmationAttempt>()
                .Query
                .FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber && x.Code == code);

            if (attempt is null || !attempt.IsValid()) return result;

            user.PhoneNumberConfirmed = true;
            _unitOfWork.Repository<ApplicationUser>().Update(user);
            attempt.MarkUsed();
            _unitOfWork.Repository<PhoneConfirmationAttempt>().Update(attempt);
            await _unitOfWork.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }
    }
}
