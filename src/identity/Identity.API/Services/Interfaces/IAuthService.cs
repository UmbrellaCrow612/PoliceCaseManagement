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
    }

    public class LoginResult
    {
        public string? LoginAttemptId { get; set; } = null;
        public ICollection<LoginError> Errors { get; set; } = [];
        public bool Succeeded { get; set; } = false;
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
        public Tokens Tokens {  get; set; } = new(); 
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
        public Tokens Tokens{ get; set; } = new();
    }

    public class TwoFactorEmailValidationError()
    {
        public required int Code { get; set;}
        public required string Message { get; set;}
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
