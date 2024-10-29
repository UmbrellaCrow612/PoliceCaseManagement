using PoliceCaseManagement.Core.Entities.Joins;

namespace PoliceCaseManagement.Core.Entities
{
    public class Role
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Name { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = [];
    }
}
