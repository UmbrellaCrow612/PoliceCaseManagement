namespace Identity.Core.Models
{
    public class EmailVerificationAttempt
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;
        public required string Email { get; set; }
        public required string Code { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public required DateTime ExpiresAt { get; set; }
        public DateTime? UsedAt { get; set; }
        public bool IsUsed { get; set; } = false;

        public bool IsValid()
        {
            if (IsUsed is true || UsedAt.HasValue)
            {
                return false;
            }

            if (DateTime.UtcNow > ExpiresAt)
            {
                return false;
            }

            return true;
        }

        public void MarkUsed()
        {
            IsUsed = true;
            UsedAt = DateTime.UtcNow;
        }
    }
}
