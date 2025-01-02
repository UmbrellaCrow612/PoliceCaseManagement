namespace Challenge.Core.Models
{
    public class ChallengeToken
    {
        /// <summary>
        /// GUID of the token, be same as the JWT jti you issue
        /// </summary>
        public required string Id { get; set; }

        public required DateTime ExpiresAt { get; set; }

        public required string UserId { get; set; }

        public required string ChallengeClaimId { get; set; }

        public required string ChallengeClaimName { get; set; }

        public bool IsExpired()
        {
            return ExpiresAt < DateTime.UtcNow;
        }

    }
}
