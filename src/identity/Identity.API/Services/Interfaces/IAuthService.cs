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

        Task<ValidateTOTPResult> ValidateTOTP(string email, string code, DeviceInfo deviceInfo);

        Task<SendResetPasswordResult> SendResetPassword(string email);

        Task<ValidateResetPasswordResult> ValidateResetPassword(string code, string newPassword);

        Task<SendConfirmationEmailResult> SendConfirmationEmail(string email);

        Task<ConfirmEmailResult> ConfirmEmail(string email, string code);

        Task<SendUserDeviceChallengeResult> SendUserDeviceChallenge(string email, DeviceInfo info);

        Task<ValidateUserDeviceChallengeResult> ValidateUserDeviceChallenge(string email, string code);
    }

    public class ValidateUserDeviceChallengeResult
    {
        public bool Succeeded { get; set; } = false;
    }

    public class SendUserDeviceChallengeResult
    {
        public bool Succeeded { get; set; } = false;
    }

        public class ConfirmEmailResult
    {
        public bool Succeeded { get; set; } = false;
    }
    public class SendConfirmationEmailResult
    {
        public bool Succeeded { get; set; } = false;
    }

    public class ValidateResetPasswordResult
    {
        public bool Succeeded { get; set; } = false;
    }

    public class SendResetPasswordResult
    {
        public bool Succeeded { get; set; } = false;
    }

    public class ValidateTOTPResult
    {
        public bool Succeeded { get; set; } = false;
        public Tokens Tokens { get; set; } = new();
    }

    public class SetUpTOTPResult
    {
        public bool Succeeded { get; set; } = false;
        public byte[] TotpSecretQrCodeBytes { get; set; } = [];
    }

    public class LoginResult
    {
        public string? LoginAttemptId { get; set; } = null;
        public ICollection<LoginError> Errors { get; set; } = [];
        public bool Succeeded { get; set; } = false;

        public void AddError(int code, string message, string? redirectUrl = null)
        {
            Errors.Add(new LoginError
            {
                Code = code,
                Message = message,
                RedirectUrl = redirectUrl
            });
        }
    }

    public class LoginError
    {
        public required int Code { get; set; }
        public required string Message { get; set; }
        public string? RedirectUrl { get; set; } = null;
    }

    public class TwoFactorSmsSentResult
    {
        public ICollection<TwoFactorSmsSentResultError> Errors { get; set; } = [];
        public bool Succeeded { get; set; } = false;
    }

    public class TwoFactorSmsSentResultError
    {
        public required int Code { get; set; }
        public required string Message { get; set; }
    }

    public class TwoFactorSmsValidationResult
    {
        public ICollection<TwoFactorSmsValidationResultError> Errors { get; set; } = [];
        public bool Succeeded { get; set; } = false;
        public Tokens Tokens { get; set; } = new();
    }

    public class TwoFactorSmsValidationResultError
    {
        public required int Code { get; set; }

        public required string Message { get; set; }
    }

    public class TwoFactorEmailSentResult
    {
        public ICollection<TwoFactorEmailSentResultError> Errors { get; set; } = [];
        public bool Succeeded { get; set; } = false;
    }

    public class TwoFactorEmailSentResultError
    {
        public required int Code { get; set; }
        public required string Message { get; set; }
    }

    public class TwoFactorEmailValidationResult()
    {
        public ICollection<TwoFactorEmailValidationError> Errors { get; set; } = [];
        public bool Succeeded { get; set; } = false;
        public Tokens Tokens { get; set; } = new();
    }

    public class TwoFactorEmailValidationError()
    {
        public required int Code { get; set; }
        public required string Message { get; set; }
    }

    public class RefreshTokenResult
    {
        public bool Succeeded { get; set; } = false;
        public ICollection<RefreshTokenResultError> Errors { get; set; } = [];
        public Tokens Tokens { get; set; } = new();
    }

    public class RefreshTokenResultError
    {

    }

    public class LogoutResult
    {
        public bool Succeeded { get; set; } = false;
        public ICollection<LogoutResultError> Errors { get; set; } = [];
    }

    public class LogoutResultError
    {

    }

    public class SendMagicLinkResult
    {
        public bool Succeeded { get; set; } = false;
        public ICollection<SendMagicLinkResultError> Errors { get; set; } = [];
    }

    public class SendMagicLinkResultError
    {

    }

    public class ValidateMagicLinkResult
    {
        public bool Succeeded { get; set; } = false;
        public Tokens Tokens { get; set; } = new();
        public ICollection<ValidateMagicLinkResultError> Errors { get; set; } = [];
    }

    public class ValidateMagicLinkResultError
    {

    }

    public class SendOTPResult
    {
        public bool Succeeded { get; set; } = false;
    }

    public class ValidateOTPResult
    {
        public bool Succeeded { get; set; } = false;
        public Tokens Tokens { get; set; } = new();
    }



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
