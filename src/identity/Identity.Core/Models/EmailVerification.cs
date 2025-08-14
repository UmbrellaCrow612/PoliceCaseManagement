namespace Identity.Core.Models
{
    /// <summary>
    /// Model to track the verification attempts of a user's email
    /// </summary>
    public class EmailVerification
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
     
        /// <summary>
        /// The email it is sent to
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// The code that was sent
        /// </summary>
        public required string Code { get; set; }

        /// <summary>
        /// When it was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// A time in the future which is past will make it expired and invalid
        /// </summary>
        public required DateTime ExpiresAt { get; set; }

        /// <summary>
        /// When the attempt was used
        /// </summary>
        public DateTime? UsedAt { get; set; }
     

        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;


        /// <summary>
        /// Helper method to tell you if a attempt is valid or not
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
        /// makr a attempt as used
        /// </summary>
        public void MarkUsed()
        {
            UsedAt = DateTime.UtcNow;
        }

    }
}
