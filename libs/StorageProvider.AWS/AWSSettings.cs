using System.ComponentModel.DataAnnotations;

namespace StorageProvider.AWS
{
    /// <summary>
    /// Settings for AWS from the config file - uses options pattern
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