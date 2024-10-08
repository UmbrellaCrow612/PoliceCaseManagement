using Microsoft.AspNetCore.Identity;
using PoliceCaseManagement.Core.Entities.Interfaces;

namespace PoliceCaseManagement.Core.Entities
{
    public class User : IdentityUser, ISoftDeletable
    {
        public DateTime? DeletedAt { get; set; } = null;
        public bool IsDeleted { get; set; } = false;
        public string? DeletedById { get; set; } = null;
        public User? DeletedBy { get; set; } = null;
    }
}