using System.ComponentModel.DataAnnotations;

namespace SMS.Settings
{
    public class SmsSettings
    {
        [Required(ErrorMessage = "AccountSid is required for sms.")]
        public required string AccountSid { get; set; }

        [Required(ErrorMessage = "AuthToken is required for sms.")]
        public required string AuthToken { get; set; }

        [Required(ErrorMessage = "FromPhoneNumber is required for sms.")]
        public required string FromPhoneNumber { get; set; }
    }
}
