﻿using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class ResetPasswordRequestDto
    {
        [EmailAddress]
        [Required]
        public required string Email { get; set; }
    }
}
