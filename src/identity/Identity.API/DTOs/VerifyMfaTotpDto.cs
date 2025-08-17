namespace Identity.API.DTOs
{
    public class VerifyMfaTotpDto
    {
        public required string LoginId { get; set; }
        public required string Code { get; set; }
    }
}
