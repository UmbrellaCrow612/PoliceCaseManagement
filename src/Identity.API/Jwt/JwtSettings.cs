namespace Identity.Api.Jwt
{
    /// <summary>
    /// Structure of the JWT Settings in app settings json
    /// </summary>
    public class JwtSettings
    {
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public required string SecretKey { get; set; }
        public required int AccessTokenExpiryMinutes { get; set; }
        public required int RefreshTokenExpiryDays { get; set; }
    }
}
