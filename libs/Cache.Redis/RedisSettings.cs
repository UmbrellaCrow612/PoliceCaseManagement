using System.ComponentModel.DataAnnotations;

namespace Cache.Redis
{
    /// <summary>
    /// Represents the settings needed for redis to be configured - read from app settings, validated and added to the DI
    /// </summary>
    public class RedisSettings
    {
        /// <summary>
        /// The Connection string to Redis not including password
        /// </summary>
        [Required]
        public required string Connection { get; set; }

        /// <summary>
        /// The password
        /// </summary>
        [Required]
        public required string Password { get; set; }
    }
}
