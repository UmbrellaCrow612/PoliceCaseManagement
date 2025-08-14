namespace Identity.Core.Models
{
    /// <summary>
    /// Represents a MFA attempt for a login using SMS
    /// </summary>
    public class TwoFactorSms
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// When it was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// If it was used and when
        /// </summary>
        public DateTime? UsedAt { get; set; } = null;

        /// <summary>
        /// When it is considered stale and invalid
        /// </summary>
        public required DateTime ExpiresAt { get; set; }

        /// <summary>
        /// The code that was sent
        /// </summary>
        public required string Code { get; set; }

        /// <summary>
        /// The number it was sent to
        /// </summary>
        public required string PhoneNumber { get; set; }


        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;

        public required string LoginId { get; set; }
        public Login? Login { get; set; } = null;


        /// <summary>
        /// Helper method to check if it is still valid - checks it it has not been used or expired
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            if (UsedAt.HasValue)
            {
                return false;
            }

            if (DateTime.UtcNow > ExpiresAt)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Helper to mark the attempt as used
        /// </summary>

        public void MarkUsed()
        {
            UsedAt = DateTime.UtcNow;
        }
    }
}
