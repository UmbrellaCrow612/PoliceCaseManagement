using Microsoft.AspNetCore.Identity;

namespace Identity.Infrastructure.Data.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string? DepartmentId { get; set; } = null;
        public Department? Department { get; set; } = null;

        public ICollection<LoginAttempt> LoginAttempts { get; set; } = [];
        public ICollection<Token> Tokens { get; set; } = [];
        public ICollection<PasswordResetAttempt> PasswordResetAttempts { get; set; } = [];
        public ICollection<SecurityAudit> SecurityAudits { get; set; } = [];
    }
}
