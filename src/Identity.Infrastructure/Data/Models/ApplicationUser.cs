﻿using Microsoft.AspNetCore.Identity;

namespace Identity.Infrastructure.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Token> Tokens { get; set; } = [];
        public ICollection<PasswordResetAttempt> PasswordResetAttempts { get; set; } = [];
    }
}
