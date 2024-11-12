using Microsoft.AspNetCore.Identity;

namespace Identity.Core
{
    public class ApplicationUser : IdentityUser
    {
        public string? RefreshToken { get; set; } = null;
    }
}
