namespace Identity.API.DTOs
{
    public class VerifyDeviceDto
    {
        public required string Code { get; set; }
        public required string Email { get; set; }
    }
}
