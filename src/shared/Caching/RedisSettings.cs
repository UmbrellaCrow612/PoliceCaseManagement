using System.ComponentModel.DataAnnotations;

namespace Caching
{
    /// <summary>
    /// Settings object from the app settings for the Redis settings - can be accessed through IOptions through the DI
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
