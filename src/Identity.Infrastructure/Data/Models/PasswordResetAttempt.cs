namespace Identity.Infrastructure.Data.Models
{
    public class PasswordResetAttempt
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;
        public required DateTime ValidSessionTime { get; set; }
        public required string Code { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsSuccessful { get; set; } = false;
        public bool IsRevoked { get; set; } = false;
    }
}
