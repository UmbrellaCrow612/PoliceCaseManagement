using System.ComponentModel.DataAnnotations;

namespace Email.Service.Settings
{
    public class EmailSettings
    {
        [Required]
        public required string FromEmail { get; set; }

        [Required]
        public required SendGridSettings SendGridSettings { get; set; }
    }

    public class SendGridSettings
    {
        [Required]
        public required string APIKey { get; set; }
    }
}
