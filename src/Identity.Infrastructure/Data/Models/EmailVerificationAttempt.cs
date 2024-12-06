namespace Identity.Infrastructure.Data.Models
{
    public class EmailVerificationAttempt
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;
        public required string Email {  get; set; }
        public required string Code { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UsedAt { get; set; }
        public bool IsUsed { get; set; } = false;

        public bool IsValid(double allowedTimeWindow = 3)
        {
            return !IsUsed && CreatedAt.AddMinutes(allowedTimeWindow) > DateTime.UtcNow;
        }

        public void MarkUsed()
        {
            IsUsed = true;
            UsedAt = DateTime.UtcNow;
        }
    }
}
