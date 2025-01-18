using System.ComponentModel.DataAnnotations;

namespace Identity.API.Settings
{
    public class IdentityApplicationRedirectUrls
    {
        [Required]
        public required string EmailConfirmationUrl {  get; set; }

        [Required]
        public required string PhoneConfirmationUrl { get; set; }

        [Required]
        public required string DeviceConfirmationUrl { get; set; }
    }
}
