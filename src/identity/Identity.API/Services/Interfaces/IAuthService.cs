using Identity.Core.Models;

namespace Identity.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResult> LoginAsync(string email, string password, DeviceInfo deviceInfo);

        Task<TwoFactorSmsSentResult> SendTwoFactorSmsVerificationCodeAsync(string loginAttemptId);

        Task<TwoFactorSmsValidationResult> ValidateTwoFactorSmsCodeAsync(string loginAttemptId, string code, DeviceInfo deviceInfo);

        Task<TwoFactorEmailSentResult> SendTwoFactorEmailVerificationCodeAsync(string loginAttemptId);

        Task<TwoFactorEmailValidationResult> ValidateTwoFactorEmailCodeAsync(string loginAttemptId, string code, DeviceInfo deviceInfo);

        Task<(bool isValid, LoginAttempt? loginAttempt)> ValidateLoginAttemptAsync(string loginAttemptId);

        Task<(bool isTrusted, UserDevice? userDevice)> ValidateDeviceAsync(string userId, DeviceInfo info);

        Task<RefreshTokenResult> RefreshTokens(string userId, string tokenId, string refreshToken, DeviceInfo deviceInfo);

        Task<LogoutResult> LogoutAsync(string userId);

        Task<SendMagicLinkResult> SendMagicLink(string email, DeviceInfo device);

        Task<ValidateMagicLinkResult> ValidateMagicLink(string code, DeviceInfo device);

        Task<SendOTPResult> SendOTP(OTPMethod method, OTPCreds creds, DeviceInfo device);

        Task<ValidateOTPResult> ValidateOTP(OTPMethod method, OTPCreds creds, string code, DeviceInfo device);

        Task<SetUpTOTPResult> SetUpTOTP(string userId, DeviceInfo deviceInfo);

        Task<ValidateTOTPResult> ValidateTOTP(string code, string loginAttemptId, DeviceInfo deviceInfo);

        Task<SendResetPasswordResult> SendResetPassword(string email);

        Task<ValidateResetPasswordResult> ValidateResetPassword(string code, string newPassword);

        Task<SendConfirmationEmailResult> SendConfirmationEmail(string email);

        Task<ConfirmEmailResult> ConfirmEmail(string email, string code);

        Task<SendUserDeviceChallengeResult> SendUserDeviceChallenge(string email, DeviceInfo info);

        Task<ValidateUserDeviceChallengeResult> ValidateUserDeviceChallenge(string email, string code);

        Task<SendPhoneConfirmationResult> SendPhoneConfirmation(string phoneNumber);

        Task<ConfirmPhoneNumberResult> ConfirmPhoneNumber(string phoneNumber, string code);

        Task GenerateNewTOTPBackUpCodes(string userId);

        Task<ValidateTOTPBackUpCodeResult> ValidateTOTPBackUpCode(string userId, string code);
    }

    public class AuthError
    {
        public required int Code { get; set; }
        public required string Message { get; set; }
        public string? RedirectUrl { get; set; }
    }

    // Base result class for all authentication results
    public class AuthResult
    {
        public bool Succeeded { get; set; } = false;
        public ICollection<AuthError> Errors { get; private set; } = [];

        public void AddError(int code, string message, string? redirectUrl = null)
        {
            Errors.Add(new AuthError
            {
                Code = code,
                Message = message,
                RedirectUrl = redirectUrl
            });
        }
    }

    public class ValidateTOTPBackUpCodeResult : AuthResult
    {

    }

    public class TokenAuthResult : AuthResult
    {
        public Tokens Tokens { get; set; } = new();
    }

    public class LoginResult : AuthResult
    {
        public string? LoginAttemptId { get; set; }
    }

    public class TwoFactorSmsSentResult : AuthResult { }

    public class TwoFactorSmsValidationResult : TokenAuthResult { }

    public class TwoFactorEmailSentResult : AuthResult { }

    public class TwoFactorEmailValidationResult : TokenAuthResult { }

    public class RefreshTokenResult : TokenAuthResult { }

    public class LogoutResult : AuthResult { }

    public class SendMagicLinkResult : AuthResult { }

    public class ValidateMagicLinkResult : TokenAuthResult { }

    public class SendOTPResult : AuthResult
    {
        public byte[] QrCodeBytes { get; set; } = [];
    }

    public class ValidateOTPResult : TokenAuthResult { }

    public class SetUpTOTPResult : AuthResult
    {
        public byte[] TotpSecretQrCodeBytes { get; set; } = [];
        public ICollection<string> BackUpCodes { get; set; } = [];
    }

    public class ValidateTOTPResult : TokenAuthResult { }

    public class SendResetPasswordResult : AuthResult { }

    public class ValidateResetPasswordResult : AuthResult { }

    public class SendConfirmationEmailResult : AuthResult { }

    public class ConfirmEmailResult : AuthResult { }

    public class SendUserDeviceChallengeResult : AuthResult { }

    public class ValidateUserDeviceChallengeResult : AuthResult { }

    public class SendPhoneConfirmationResult : AuthResult { }

    public class ConfirmPhoneNumberResult : AuthResult { }


    public class DeviceInfo
    {
        public required string IpAddress { get; set; }
        public required string UserAgent { get; set; }
        public required string DeviceFingerPrint { get; set; }
    }

    public class Tokens
    {
        public string JwtBearerToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
