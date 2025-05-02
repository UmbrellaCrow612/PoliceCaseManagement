using System.ComponentModel.DataAnnotations;

namespace Email.Worker.Settings
{
    /// <summary>
    /// Section from the app settings added to the DI with some validation
    /// </summary>
    public class EmailSettings
    {
        [Required, EmailAddress]
        public string From { get; set; } = default!;

        [Required]
        public string SmtpHost { get; set; } = default!;

        [Range(1, 65535)]
        public int SmtpPort { get; set; }

        [Required]
        public string Username { get; set; } = default!;

        [Required]
        public string Password { get; set; } = default!;
    }
}
