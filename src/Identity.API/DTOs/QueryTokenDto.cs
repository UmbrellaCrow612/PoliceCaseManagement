namespace Identity.API.DTOs
{
    public class QueryTokenDto
    {
        public required string Id { get; set; }
        public required string UserId { get; set; }
        public required DateTime RefreshTokenExpiresAt { get; set; }
        public bool IsRevoked { get; set; } 
        public DateTime? RevokedAt { get; set; } 
        public string? RevokedReason { get; set; } 
        public bool IsBlackListed { get; set; } 
        public DateTime CreatedAt { get; set; } 
        public required string DeviceInfoId { get; set; }
    }
}
