namespace Identity.Infrastructure.Data.Models
{
    public class RolePermission
    {
        public required string PermissionId { get; set; }
        public required string RoleId { get; set; }

        public Permission? Permission { get; set; } = null;
        public ApplicationRole? Role { get; set; } = null;
    }
}
