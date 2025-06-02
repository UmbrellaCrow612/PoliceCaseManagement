using System.ComponentModel.DataAnnotations;
using Identity.Core.Models;

namespace Identity.API.DTOs
{
    public class ValidateOtpDto
    {
        [Required]
        [EnumDataType(typeof(OTPMethod))]
        public required OTPMethod OTPMethod { get; set; }

        public required OTPCreds OTPCreds { get; set; }

        [Required]
        public required string Code { get; set; }
    }
}
