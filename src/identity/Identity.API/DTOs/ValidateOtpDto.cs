﻿using Identity.Core.Models;
using System.ComponentModel.DataAnnotations;

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
