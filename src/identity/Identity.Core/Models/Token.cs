namespace Identity.Core.Models
{
    /// <summary>
    /// Represents a token session for a user - contains information about JWt tokens issues to the user
    /// </summary>
    public class Token
    {
        /// <summary>
        /// A unique ID - represents the refresh token value it self which is sent to the client as the refresh token value
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// When it will be considered stale and invalid
        /// </summary>
        public required DateTime ExpiresAt { get; set; }

        /// <summary>
        /// If a token is revoked and the time it was
        /// </summary>
        public DateTime? RevokedAt { get; set; } = null;

        /// <summary>
        /// When it was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// If and when the token was used for a new issue of tokens
        /// </summary>
        public DateTime? UsedAt { get; set; } = null;


        public required string DeviceId { get; set; }
        public Device? Device { get; set; } = null;

        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;


        /// <summary>
        /// Checks if a Token is valid - it has not been revoked or expired or the device it matches the one it was issued for
        /// </summary>
        /// <param name="deviceId">The device the token was issued to</param>
        public bool IsValid(string deviceId)
        {
            if (RevokedAt.HasValue)
            {
                return false;
            }

            if (UsedAt.HasValue)
            {
                return false;
            }

            if (DateTime.UtcNow > ExpiresAt)
            {
                return false;
            }

            if (DeviceId != deviceId)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Helper method to revoke the current token
        /// </summary>
        public void Revoke()
        {
            RevokedAt = DateTime.UtcNow;
        }

        public void MarkUsed()
        {
            UsedAt = DateTime.UtcNow;
        }
    }
}
