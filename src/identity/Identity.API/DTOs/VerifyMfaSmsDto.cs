namespace Identity.API.DTOs
{
    public class VerifyMfaSmsDto
    {
        public required string LoginId { get; set; }
        public required string Code { get; set; }
    }
}
