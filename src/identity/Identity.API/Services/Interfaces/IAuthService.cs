namespace Identity.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResult> Login(string email, string password, HttpRequest request);
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
}
