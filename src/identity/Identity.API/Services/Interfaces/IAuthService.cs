namespace Identity.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResult> LoginAsync(string email, string password, DeviceInfo deviceInfo);

        Task<TwoFactorSmsVerificationResult> SendTwoFactorSmsVerificationCodeAsync(string loginAttemptId);

        Task<TwoFactorSmsVerificationResult> ValidateTwoFactorSmsCodeAsync(string loginAttemptId, string code, DeviceInfo deviceInfo);

        Task<JwtBearerTokenResult> GenerateTokensAsync(string loginAttemptId, DeviceInfo deviceInfo);
    }

    public class TwoFactorSmsVerificationResult
    {
        public ICollection<TwoFactorSmsVerificationError> Errors { get; set; } = [];
        public bool Succeeded { get; set; } = false;
    }

    public class TwoFactorSmsVerificationError
    {
        public required int Code { get; set; }
        public required string Message { get; set; }
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

    public class DeviceInfo
    {
        public required string IpAddress { get; set; }
        public required string UserAgent { get; set; }
        public required string DeviceFingerPrint { get; set; }
    }

    public class JwtBearerTokenResult
    {
        public required Tokens Tokens { get; set; }
        public ICollection<JwtBearerTokenError> Errors { get; set; } = [];
        public bool Succeeded { get; set; } = false;
    }

    public class JwtBearerTokenError
    {
        public required int Code { get; set; }
        public required string Message { get; set; }
    }

    public class Tokens
    {
        public string JwtBearerToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
