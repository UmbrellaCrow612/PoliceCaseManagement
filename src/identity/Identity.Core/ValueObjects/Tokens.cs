namespace Identity.Core.ValueObjects
{
    /// <summary>
    /// Shape in which we sent tokens to UI through API 
    /// </summary>
    public class Tokens
    {
        public string JwtBearerToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
