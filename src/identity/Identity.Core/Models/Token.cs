namespace Identity.Core.Models
{
    public class Token
    {
        public required string Id { get; set; }
        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;
        public required string RefreshToken { get; set; }
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
        /// <param name="refreshToken">The refresh token that the current token was issued with</param>
        /// <param name="deviceId">The device the token was issued to</param>
        /// <returns>True or false</returns>
        public bool IsValid(string refreshToken, string deviceId)
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

            if (RefreshToken != refreshToken)
            {
                return false;
            }

            if (UserDeviceId != deviceId)
            {
                return false;
            }

            return true;
        }

        public void Revoke()
        {
            IsRevoked = true;
        }

        public void BlackList()
        {
            IsBlackListed = true;
        }
    }
}
