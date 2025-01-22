namespace Identity.Core.Models
{
    public class UserDeviceChallengeAttempt
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Email { get; set; }
        public required string Code { get; set; }
        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;
        public required string UserDeviceId { get; set; }
        public UserDevice? UserDevice { get; set; } = null;
        public bool IsUsed { get; set; } = false;
        public DateTime? UsedAt { get; set; } = null;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public required DateTime ExpiresAt { get; set; }

        public bool IsValid()
        {
            if (IsUsed)
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
