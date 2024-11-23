using Identity.Infrastructure.Data.Models.enums;

namespace Identity.Infrastructure.Data.Models
{
    public class Permission
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Name { get; set; }
        public string? Description { get; set; } = null;
        public required SystemResource Resource { get; set; }
        public required SystemAction Action { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; } = [];
    }
}
