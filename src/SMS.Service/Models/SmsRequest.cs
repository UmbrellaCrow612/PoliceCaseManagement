namespace SMS.Service.Models
{
    public class SmsRequest
    {
        public required string ToPhoneNumber { get; set; }
        public required string Message { get; set; }
    }
}
