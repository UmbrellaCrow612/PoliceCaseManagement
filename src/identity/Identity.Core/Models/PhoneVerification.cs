namespace Identity.Core.Models
{
    /// <summary>
    /// Represents a attempt made to verify a users phone number
    /// </summary>
    public class PhoneVerification
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// The code sent
        /// </summary>
        public required string Code { get; set; }

        /// <summary>
        /// When it was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// A time when it is considered expired
        /// </summary>
        public required DateTime ExpiresAt { get; set; }
      

        /// <summary>
        /// The number it was sent for
        /// </summary>
        public required string PhoneNumber { get; set; }

        /// <summary>
        /// If it was used and when
        /// </summary>
        public DateTime? UsedAt { get; set; } = null;

        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;

        /// <summary>
        /// Helper to mark the attempt as used
        /// </summary>
        public void MarkUsed()
        {
            UsedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Checks if a attempt is still valid - if it has not been used or expired
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
    }
}
