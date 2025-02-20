using Authorization.Core;
using Identity.Application.Helpers;
using Identity.Application.Settings;
using Identity.Application.Codes;
using Identity.Core.Models;
using Identity.Core.Repositorys;
using Identity.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OtpNet;
using System.Data;
using System.Security.Cryptography;
using Identity.Core.ValueObjects;

namespace Identity.Application.Implamentations
{
    internal class AuthService(UserManager<ApplicationUser> userManager, DeviceManager deviceManager, IOptions<TimeWindows> options, JwtBearerHelper jwtBearerHelper, IOptions<JwtBearerOptions> jwtBearerOptions, IUnitOfWork unitOfWork, ILogger<AuthService> logger) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly DeviceManager _deviceManager = deviceManager;
        private readonly TimeWindows _timeWindows = options.Value;
        private readonly JwtBearerHelper _jwtBearerHelper = jwtBearerHelper;
        private readonly JwtBearerOptions _JwtBearerOptions = jwtBearerOptions.Value;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<AuthService> _logger = logger;

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
            _logger.LogInformation("Login attempt started for email: {Email} from IP: {IpAddress}", email, deviceInfo.IpAddress);

            var result = new LoginResult();
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                _logger.LogWarning("Login failed: User does not exist. Email: {Email}, IP: {IpAddress}", email, deviceInfo.IpAddress);
                result.AddError(BusinessRuleCodes.UserDoesNotExist);
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
                loginAttempt.FailureReason = "Incorrect password.";
                await _unitOfWork.SaveChangesAsync();
                _logger.LogWarning("Login failed: Incorrect credentials. UserId: {UserId}, IP: {IpAddress}", user.Id, deviceInfo.IpAddress);
                result.AddError(BusinessRuleCodes.IncorrectCredentials);
                return result;
            }

            if (user.LockoutEnabled && user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
            {
                loginAttempt.FailureReason = "Account locked.";
                await _unitOfWork.SaveChangesAsync();
                _logger.LogWarning("Login failed: Account locked. UserId: {UserId}, IP: {IpAddress}", user.Id, deviceInfo.IpAddress);
                result.AddError(BusinessRuleCodes.AccountLocked);
                return result;
            }

            if (!user.EmailConfirmed)
            {
                loginAttempt.FailureReason = "Email not confirmed.";
                await _unitOfWork.SaveChangesAsync();
                _logger.LogWarning("Login failed: Email not confirmed. UserId: {UserId}, IP: {IpAddress}", user.Id, deviceInfo.IpAddress);
                result.AddError(BusinessRuleCodes.EmailNotConfirmed);
                return result;
            }

            if (!user.PhoneNumberConfirmed)
            {
                loginAttempt.FailureReason = "Phone number not confirmed.";
                await _unitOfWork.SaveChangesAsync();
                _logger.LogWarning("Login failed: Phone number not confirmed. UserId: {UserId}, IP: {IpAddress}", user.Id, deviceInfo.IpAddress);
                result.AddError(BusinessRuleCodes.PhoneNumberNotConfirmed);
                return result;
            }

            var device = await _deviceManager.GetRequestingDevice(user.Id, deviceInfo.DeviceFingerPrint, deviceInfo.UserAgent);
            if (device is null || !device.Trusted())
            {
                loginAttempt.FailureReason = "Untrusted device.";
                await _unitOfWork.SaveChangesAsync();
                _logger.LogWarning("Login failed: Untrusted device. UserId: {UserId}, IP: {IpAddress}, DeviceFingerprint: {DeviceFingerprint}",
                    user.Id, deviceInfo.IpAddress, deviceInfo.DeviceFingerPrint);
                result.AddError(BusinessRuleCodes.DeviceNotConfirmed);
                return result;
            }

            loginAttempt.Status = LoginStatus.TwoFactorAuthenticationReached;
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Login successful: UserId: {UserId}, IP: {IpAddress}, proceeding to 2FA.", user.Id, deviceInfo.IpAddress);

            result.Succeeded = true;
            return result;
        }

        public async Task<TwoFactorEmailSentResult> SendTwoFactorEmailVerificationCodeAsync(string loginAttemptId)
        {
            var result = new TwoFactorEmailSentResult();

            var loginAttempt = await _unitOfWork.Repository<LoginAttempt>().FindByIdAsync(loginAttemptId);
            if (loginAttempt is null || !loginAttempt.IsValid())
            {
                result.AddError(BusinessRuleCodes.LoginAttemptNotValid);
                return result;
            }

            var user = await _userManager.FindByIdAsync(loginAttempt.UserId);
            if (user is null)
            {
                result.AddError(BusinessRuleCodes.UserDoesNotExist);
                return result;
            }

            if (!user.IsEmailConfirmed())
            {
                result.AddError(BusinessRuleCodes.EmailNotConfirmed);
                return result;
            }

            var validRecentTwoFactorEmailAttemptExists = await _unitOfWork.Repository<TwoFactorEmailAttempt>()
                .Query
                .Where(x => x.ExpiresAt > DateTime.UtcNow && x.LoginAttemptId == loginAttemptId && x.IsSuccessful == false)
                .OrderBy(x => x.CreatedAt)
                .AnyAsync();

            if (validRecentTwoFactorEmailAttemptExists)
            {
                result.AddError(BusinessRuleCodes.ValidTwoFactorEmailAttemptExists);
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
            if (loginAttempt is null || !loginAttempt.IsValid())
            {
                result.AddError(BusinessRuleCodes.LoginAttemptNotValid);
                return result;
            }

            var user = await _userManager.FindByIdAsync(loginAttempt.UserId);
            if (user is null)
            {
                result.AddError(BusinessRuleCodes.UserDoesNotExist);
                return result;
            }

            if (!user.IsPhoneNumberConfirmed())
            {
                result.AddError(BusinessRuleCodes.PhoneNumberNotConfirmed);
                return result;
            }

            var validRecentTwoFactorSmsAttemptExist = await _unitOfWork.Repository<TwoFactorSmsAttempt>()
                .Query
                .Where(x => x.ExpiresAt > DateTime.UtcNow && x.LoginAttemptId == loginAttemptId && x.IsSuccessful == false)
                .OrderBy(x => x.ExpiresAt)
                .AnyAsync();

            if (validRecentTwoFactorSmsAttemptExist)
            {
                result.AddError(BusinessRuleCodes.ValidTwoFactorSmsAttemptExists);
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
            if (!isValid || loginAttempt is null)
            {
                result.AddError(BusinessRuleCodes.LoginAttemptNotValid);
                return result;
            }

            var user = await GetUserByIdAsync(loginAttempt.UserId);
            if (user is null)
            {
                result.AddError(BusinessRuleCodes.UserDoesNotExist);
                return result;
            }

            var (isTrusted, userDevice) = await ValidateDeviceAsync(user.Id, deviceInfo);
            if (!isTrusted || userDevice is null)
            {
                result.AddError(BusinessRuleCodes.DeviceNotConfirmed);
                return result;
            }

            var twoFactorEmailAttempt = await _unitOfWork.Repository<TwoFactorEmailAttempt>()
                .Query
                .Where(x => x.LoginAttemptId == loginAttemptId && x.Code == code)
                .FirstOrDefaultAsync();

            if (twoFactorEmailAttempt is null || !twoFactorEmailAttempt.IsValid())
            {
                result.AddError(BusinessRuleCodes.TwoFactorEmailAttemptInvalid);
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
            if (loginAttempt is null || !loginAttempt.IsValid())
            {
                result.AddError(BusinessRuleCodes.LoginAttemptNotValid);
                return result;
            }

            var user = await _userManager.FindByIdAsync(loginAttempt.UserId);
            if (user is null)
            {
                result.AddError(BusinessRuleCodes.UserDoesNotExist);
                return result;
            }

            var (isTrusted, userDevice) = await ValidateDeviceAsync(user.Id, deviceInfo);
            if (userDevice is null || !isTrusted)
            {
                result.AddError(BusinessRuleCodes.DeviceNotConfirmed);
                return result;
            }

            var smsAttempt = await _unitOfWork.Repository<TwoFactorSmsAttempt>().Query.Where(x => x.LoginAttemptId == loginAttemptId && x.Code == code).FirstOrDefaultAsync();
            if (smsAttempt is null || !smsAttempt.IsValid())
            {
                result.AddError(BusinessRuleCodes.TwoFactorSmsAttemptInvalid);
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

        public async Task<(bool isTrusted, UserDevice? userDevice)> ValidateDeviceAsync(string userId, DeviceInfo info)
        {
            var device = await _deviceManager.GetRequestingDevice(userId, info.DeviceFingerPrint, info.UserAgent);
            if (device is null) return (false, null);

            if (!device.Trusted()) return (false, null);

            return (true, device);
        }

        public async Task<RefreshTokenResult> RefreshTokensAsync(string userId, string tokenId, string refreshToken, DeviceInfo deviceInfo)
        {
            var result = new RefreshTokenResult();

            var user = await GetUserByIdAsync(userId);
            if (user is null)
            {
                result.AddError(BusinessRuleCodes.UserDoesNotExist);
                return result;
            }

            var (isTrusted, userDevice) = await ValidateDeviceAsync(user.Id, deviceInfo);
            if (userDevice is null || !isTrusted)
            {
                result.AddError(BusinessRuleCodes.DeviceNotConfirmed);
                return result;
            }

            var storedToken = await _unitOfWork.Repository<Token>().FindByIdAsync(tokenId);
            if (storedToken is null || !storedToken.IsValid(refreshToken, userDevice.Id))
            {
                result.AddError(BusinessRuleCodes.RefreshToken);
                return result;
            }

            var roles = await _userManager.GetRolesAsync(user);

            var (jwtBearerToken, jwtBearerAcessTokenId) = _jwtBearerHelper.GenerateBearerToken(user, roles);

            var newToken = new Token
            {
                Id = jwtBearerAcessTokenId,
                RefreshToken = storedToken.RefreshToken,
                RefreshTokenExpiresAt = storedToken.RefreshTokenExpiresAt, // bind to previous refresh token and time
                UserDeviceId = userDevice.Id,
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
            if (user is null)
            {
                result.AddError(BusinessRuleCodes.UserDoesNotExist);
                return result;
            }

            var validTokens = await _unitOfWork.Repository<Token>()
                .Query
                .Where(x => x.IsRevoked == false && x.UserId == userId && x.RefreshTokenExpiresAt > DateTime.UtcNow)
                .ToArrayAsync();

            foreach (var token in validTokens)
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
            if (!isTrusted || userDevice is null) return result;

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
                if (user is null || !user.IsEmailConfirmed() || !user.IsOTPAuthEnabled()) return result;

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

                var QRCodeBytes = QRCodeHandler.GenerateOTPQRCodeBytes(newAttempt);
                result.QrCodeBytes = QRCodeBytes;

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

                // use sms service and send OTP Status
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
                        "PCMS"
                        );
            result.TotpSecretQrCodeBytes = secretAsQrCodebytes;

            var totp = new TimeBasedOneTimePassCode
            {
                UserId = user.Id,
                Secret = base32Secret
            };

            await _unitOfWork.Repository<TimeBasedOneTimePassCode>().AddAsync(totp);

            List<TimeBasedOneTimePassCodeBackupCode> backUpCodes = [];
            int backUpCodeCount = 4;

            for (int i = 0; i < backUpCodeCount; i++)
            {
                backUpCodes.Add(new TimeBasedOneTimePassCodeBackupCode
                {
                    Code = Guid.NewGuid().ToString()[..8], // sent to user without hashing then hash to store
                    TimeBasedOneTimePassCodeId = totp.Id,
                    UserId = user.Id
                });
            }
            result.BackUpCodes = backUpCodes.Select(x => x.Code).ToArray();

            await _unitOfWork.Repository<TimeBasedOneTimePassCodeBackupCode>().AddRangeAsync(backUpCodes);

            await _unitOfWork.SaveChangesAsync();

            result.Succeeded = true;

            return result;
        }

        public async Task<ValidateTOTPResult> ValidateTOTP(string code, string loginAttemptId, DeviceInfo deviceInfo)
        {
            var result = new ValidateTOTPResult();

            var (isValid, loginAttempt) = await ValidateLoginAttemptAsync(loginAttemptId);
            if (!isValid || loginAttempt is null)
            {
                result.AddError(BusinessRuleCodes.LoginAttemptNotValid);
                return result;
            }

            var user = await _userManager.FindByIdAsync(loginAttempt.UserId);
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

            loginAttempt.MarkUsed();
            _unitOfWork.Repository<LoginAttempt>().Update(loginAttempt);

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
            if (user is null || user.IsEmailConfirmed())
            {
                result.AddError(user is null ? BusinessRuleCodes.UserDoesNotExist : BusinessRuleCodes.EmailAlreadyConfirmed);
                return result;
            };

            var validRecentEmailConfirmationAttemptExists = await _unitOfWork.Repository<EmailVerificationAttempt>()
                .Query
                .Where(x => x.ExpiresAt > DateTime.UtcNow && x.UserId == user.Id && x.IsUsed == false)
                .AnyAsync();

            if (validRecentEmailConfirmationAttemptExists)
            {
                result.AddError(BusinessRuleCodes.ValidEmailConfirmationAttemptExists);
                return result;
            };

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
            if (user is null || user.IsEmailConfirmed())
            {
                result.AddError(user is null ? BusinessRuleCodes.UserDoesNotExist : BusinessRuleCodes.EmailAlreadyConfirmed);
                return result;
            };

            var attempt = await _unitOfWork.Repository<EmailVerificationAttempt>()
                .Query
                .FirstOrDefaultAsync(x => x.UserId == user.Id && x.Code == code);

            if (attempt is null || !attempt.IsValid())
            {
                result.AddError(BusinessRuleCodes.EmailConfirmation);
                return result;
            };
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
            if (user is null)
            {
                result.AddError(BusinessRuleCodes.UserDoesNotExist);
                return result;
            };

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
            if (device is null || device.IsTrusted)
            {
                result.AddError(device is null ? BusinessRuleCodes.DeviceNotConfirmed : BusinessRuleCodes.DeviceAlreadyTrusted);
                return result;
            };

            var validRecentAttemptExists = await _unitOfWork.Repository<UserDeviceChallengeAttempt>()
                .Query
                .Where(x => x.ExpiresAt > DateTime.UtcNow && x.UserId == user.Id && x.UserDeviceId == device.Id && x.IsUsed == false)
                .AnyAsync();

            if (validRecentAttemptExists)
            {
                result.AddError(BusinessRuleCodes.ValidDeviceConfirmationAttemptExists);
                return result;
            };

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
            if (attempt is null || !attempt.IsValid())
            {
                result.AddError(BusinessRuleCodes.DeviceConfirmationAttemptDoesNotExist);
                return result;
            };

            var device = await _unitOfWork.Repository<UserDevice>().FindByIdAsync(attempt.UserDeviceId);
            if (device is null || device.IsTrusted)
            {
                result.AddError(device is null ? BusinessRuleCodes.DeviceNotConfirmed : BusinessRuleCodes.DeviceAlreadyTrusted);
                return result;
            };

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
            if (user is null || user.PhoneNumberConfirmed)
            {
                result.AddError(user is null ? BusinessRuleCodes.UserDoesNotExist : BusinessRuleCodes.PhoneNumberAlreadyConfirmed);
                return result;
            }

            var validRecentPhoneConfirmationAttemptExists = await _unitOfWork.Repository<PhoneConfirmationAttempt>()
                .Query
                .Where(x => x.ExpiresAt > DateTime.UtcNow && x.UserId == user.Id && x.IsUsed == false)
                .AnyAsync();

            if (validRecentPhoneConfirmationAttemptExists)
            {
                result.AddError(BusinessRuleCodes.ValidConfirmationPhoneNumberAttemptExists);
                return result;
            };

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
            if (user is null || user.PhoneNumberConfirmed)
            {
                result.AddError(user is null ? BusinessRuleCodes.UserDoesNotExist : BusinessRuleCodes.PhoneNumberAlreadyConfirmed);
                return result;
            };

            var attempt = await _unitOfWork.Repository<PhoneConfirmationAttempt>()
                .Query
                .FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber && x.Code == code);

            if (attempt is null || !attempt.IsValid())
            {
                result.AddError(BusinessRuleCodes.ConfirmPhoneNumberAttemptDoesNotExist);
                return result;
            };

            user.PhoneNumberConfirmed = true;
            _unitOfWork.Repository<ApplicationUser>().Update(user);
            attempt.MarkUsed();
            _unitOfWork.Repository<PhoneConfirmationAttempt>().Update(attempt);
            await _unitOfWork.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public Task GenerateNewTOTPBackUpCodes(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ValidateTOTPBackUpCodeResult> ValidateTOTPBackUpCode(string userId, string code)
        {
            var result = new ValidateTOTPBackUpCodeResult();

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                result.AddError(BusinessRuleCodes.UserDoesNotExist);
                return result;
            }

            var backUpCode = await _unitOfWork.Repository<TimeBasedOneTimePassCodeBackupCode>()
                .Query
                .Where(x => x.UserId == user.Id && x.Code == code)
                .FirstOrDefaultAsync();
            if (backUpCode is null || !backUpCode.IsValid())
            {
                result.AddError(BusinessRuleCodes.BackupCodeDoesNotExist);
                return result;
            }
            backUpCode.MarkUsed();
            _unitOfWork.Repository<TimeBasedOneTimePassCodeBackupCode>().Update(backUpCode);
            await _unitOfWork.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<TurnOnTOTPResult> TurnOnTOTP(string userId)
        {
            var result = new TurnOnTOTPResult();
            var user = await GetUserByIdAsync(userId);
            if (user is null)
            {
                result.AddError(BusinessRuleCodes.UserDoesNotExist);
                return result;
            }
            if (user.IsTOTPAuthEnabled())
            {
                result.AddError(BusinessRuleCodes.TotpAuthAlreadyEnabled, "Time base one time passcodes are already turned on for this user.");
                return result;
            }
            user.TimeBasedOneTimePassCodeEnabled = true;
            _unitOfWork.Repository<ApplicationUser>().Update(user);
            await _unitOfWork.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<TurnOnOTPResult> TurnOnOTP(string userId)
        {
            var result = new TurnOnOTPResult();
            var user = await GetUserByIdAsync(userId);
            if (user is null)
            {
                result.AddError(BusinessRuleCodes.UserDoesNotExist);
                return result;
            }
            if (user.IsOTPAuthEnabled())
            {
                result.AddError(BusinessRuleCodes.OneTimePasswordAuthAlreadyEnabled);
                return result;
            }
            user.OTPAuthEnabled = true;
            _unitOfWork.Repository<ApplicationUser>().Update(user);
            await _unitOfWork.SaveChangesAsync();

            result.MarkSucceeded();
            return result;
        }

        public async Task<TurnOnMagicLinkResult> TurnOnMagicLink(string userId)
        {
            var result = new TurnOnMagicLinkResult();
            var user = await GetUserByIdAsync(userId);
            if (user is null)
            {
                result.AddError(BusinessRuleCodes.UserDoesNotExist);
                return result;
            }
            if (user.IsMagicLinkAuthEnabled())
            {
                result.AddError(BusinessRuleCodes.MagicLinkAuthAlreadyEnabled);
                return result;
            }
            user.MagicLinkAuthEnabled = true;
            _unitOfWork.Repository<ApplicationUser>().Update(user);
            await _unitOfWork.SaveChangesAsync();

            result.MarkSucceeded();
            return result;
        }

        public async Task<RegisterUserResult> RegisterUserAsync(ApplicationUser userToCreate, string password)
        {
            var result = new RegisterUserResult();
            var isPhoneNumberAlreadyTaken = await _userManager.Users.AnyAsync(x => x.PhoneNumber == userToCreate.PhoneNumber);
            if (isPhoneNumberAlreadyTaken)
            {
                result.AddError(BusinessRuleCodes.PhoneNumberAlreadyTaken);
                return result;
            }
            var r = await _userManager.CreateAsync(userToCreate, password);
            if (!r.Succeeded)
            {
                foreach (var err in r.Errors)
                {
                    var mess = $@"Identity error code: ${err.Code} description: ${err.Description}";

                    result.AddError(BusinessRuleCodes.ValidationError, mess);
                }
                return result;
            }

            result.MarkSucceeded();
            return result;
        }
    }
}
