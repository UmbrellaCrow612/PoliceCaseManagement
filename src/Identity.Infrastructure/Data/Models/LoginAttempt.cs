namespace Identity.Infrastructure.Data.Models
{
    public class LoginAttempt
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;
        public required string IpAddress { get; set; }
        public required string UserAgent { get; set; }
        public LoginStatus Status { get; set; } = LoginStatus.FAILED;
        public string? FailureReason { get; set; } = null;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<TwoFactorCodeAttempt> TwoFactorCodeAttempts { get; set; } = [];
    }

    public enum LoginStatus
    {
        SUCCESS = 0,
        FAILED = 1,
        BLOCKED = 2,
        TwoFactorAuthenticationSent = 3
    }
}
