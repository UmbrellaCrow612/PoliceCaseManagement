﻿using Microsoft.AspNetCore.Identity;

namespace Identity.Core
{
    public class ApplicationUser : IdentityUser
    {
        public string? RefreshToken { get; set; } = null;
        public DateTime? RefreshTokenExpiriesAt { get; set; } = null;
    }
}
