namespace Identity.Infrastructure.Data.Models
{
    public class Token
    {
        public required string Id { get; set; }
        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;
        public required string RefreshToken { get; set; }
        public required DateTime RefreshTokenExpiresAt { get; set; }
        public required string AccessToken { get; set; }
        public bool IsRevoked { get; set; } = false;
        public DateTime? RevokedAt { get; set; } = null;
        public string? RevokedReason { get; set; }
        public bool IsBlackListed { get; set; } = false;
    }
}
