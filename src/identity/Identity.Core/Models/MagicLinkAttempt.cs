namespace Identity.Core.Models
{
    public class MagicLinkAttempt
    {
        /// <summary>
        /// Used in the url domain/auth/magic-link?code=id
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; } = false;
        public DateTime? UsedAt { get; set; } = null;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;


        public bool IsValid()
        {
            if (DateTime.UtcNow > ExpiresAt)
            {
                return false;
            }

            if (IsUsed || UsedAt.HasValue)
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
