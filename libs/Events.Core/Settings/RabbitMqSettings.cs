using System.ComponentModel.DataAnnotations;

namespace Events.Core.Settings
{
    /// <summary>
    /// Settings object to get the section related to rabbit mq, and validate it and also provide it in the DI.
    /// </summary>
    public class RabbitMqSettings
    {
        [Required]
        public required string Host { get; set; }

        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}