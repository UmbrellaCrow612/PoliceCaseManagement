namespace Identity.API.DTOs
{
    public class UserTokensQuery
    {
        public DateTime? RefreshTokenExpiresAt { get; set; } = null;
        public bool? IsRevoked { get; set; } = null;
        public DateTime? RevokedAt { get; set; } = null;
        public string? RevokedReason { get; set; } = null;
        public bool? IsBlackListed { get; set; } = null;
        public DateTime? CreatedAt { get; set; } = null;
    }
}
