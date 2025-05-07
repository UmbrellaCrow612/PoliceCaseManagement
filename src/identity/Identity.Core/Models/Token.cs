namespace Identity.Core.Models
{
    public class Token
    {
        public required string Id { get; set; }
        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;
        public required DateTime RefreshTokenExpiresAt { get; set; }
        public bool IsRevoked { get; set; } = false;
        public DateTime? RevokedAt { get; set; } = null;
        public string? RevokedReason { get; set; } = null;
        public bool IsBlackListed { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public required string UserDeviceId { get; set; }


        /// <summary>
        /// Checks if a Token is valid
        /// </summary>
        /// <param name="deviceId">The device the token was issued to</param>
        /// <returns>True or false</returns>
        public bool IsValid(string deviceId)
        {
            if (IsRevoked)
            {
                return false;
            }

            if (IsBlackListed)
            {
                return false;
            }

            if (DateTime.UtcNow > RefreshTokenExpiresAt)
            {
                return false;
            }

            if (UserDeviceId != deviceId)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Helper method to revoke the current token
        /// </summary>
        /// <param name="reason">Reason why it is being revoked</param>
        public void Revoke(string reason)
        {
            IsRevoked = true;
            RevokedAt = DateTime.UtcNow;
            RevokedReason = reason;
        }

        public void BlackList()
        {
            IsBlackListed = true;
        }
    }
}
