using System.ComponentModel.DataAnnotations;

namespace Cases.Infrastructure.Options
{
    /// <summary>
    /// AWS settings
    /// </summary>
    public class AWSSettings
    {
        [Required]
        public required string AccessKey { get; set; }

        [Required]
        public required string SecretKey { get; set; }

        [Required]
        public required string Region { get; set; }

        [Required]
        public required string BucketName { get; set; }
    }
}
