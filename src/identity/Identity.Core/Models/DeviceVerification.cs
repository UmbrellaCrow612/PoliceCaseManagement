namespace Identity.Core.Models
{
    /// <summary>
    /// A Verification attempt made for a <see cref="ApplicationUser"/> <see cref="Device"/>
    /// </summary>
    public class DeviceVerification
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// The email sent to
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// The code
        /// </summary>
        public required string Code { get; set; }

        /// <summary>
        /// Date time it was used
        /// </summary>
        public DateTime? UsedAt { get; set; } = null;

        /// <summary>
        /// When it was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// A date in the future when it will expire and no longer be considered valid 
        /// </summary>
        public required DateTime ExpiresAt { get; set; }


        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;


        public required string DeviceId { get; set; }
        public Device? Device { get; set; } = null;



        /// <summary>
        /// Helper method to tell you if a Device Verification attempt is valid or not
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
        /// helper method to mark a attempt as used
        /// </summary>
        public void MarkUsed()
        {
            UsedAt = DateTime.UtcNow;
        }

    }
}
