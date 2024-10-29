namespace PoliceCaseManagement.Core.Entities.Joins
{
    public class UserRole
    {
        public required string UserId { get; set; }
        public required string RoleId { get; set; }

        public User? User { get; set; } = null;
        public Role? Role { get; set; } = null;
    }
}
