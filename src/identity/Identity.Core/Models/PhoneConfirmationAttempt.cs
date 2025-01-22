namespace Identity.Core.Models
{
    public class PhoneConfirmationAttempt
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Code { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public required DateTime ExpiresAt { get; set; }
        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;
        public required string PhoneNumber { get; set; }
        public bool IsUsed { get; set; } = false;
        public DateTime? UsedAt { get; set; } = null;

        public void MarkUsed()
        {
            IsUsed = true;
            UsedAt = DateTime.UtcNow;
        }

        public bool IsValid()
        {
            if (IsUsed is true)
            {
                return false;
            }

            if (DateTime.UtcNow > ExpiresAt)
            {
                return false;
            }

            return true;
        }
    }
}
