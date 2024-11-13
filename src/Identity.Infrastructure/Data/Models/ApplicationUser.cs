using Microsoft.AspNetCore.Identity;

namespace Identity.Infrastructure.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public IEnumerable<Token> Tokens { get; set; } = [];
        public string? RefreshToken { get; set; } = null;
        public DateTime? RefreshTokenExpiriesAt { get; set; } = null;
    }
}
