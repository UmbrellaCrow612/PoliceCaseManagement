using System.ComponentModel.DataAnnotations;

namespace Email.Service.Settings
{
    public class EmailSettings
    {
        [Required]
        [EmailAddress]
        public required string SenderEmail { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public required string SenderPassword { get; set; }

        [Required]
        public required string SmtpServer { get; set; }

        [Range(1, 65535, ErrorMessage = "Port must be a valid number between 1 and 65535.")]
        public required int SmtpPort { get; set; }
    }
}
