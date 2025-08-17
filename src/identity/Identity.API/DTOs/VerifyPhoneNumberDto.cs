namespace Identity.API.DTOs
{
    public class VerifyPhoneNumberDto
    {
        public required string PhoneNumber { get; set; }
        public required string Code { get; set; }
    }
}
