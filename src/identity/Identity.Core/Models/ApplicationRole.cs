using Identity.Core.Models.Joins;

namespace Identity.Core.Models
{
    /// <summary>
    /// Roles a <see cref="ApplicationUser"/> can have in the system
    /// </summary>
    public class ApplicationRole
    {
        /// <summary>
        /// Unique ID of the role
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// A unique display name for the role
        /// </summary>
        public required string Name { get; set; }


        // ef core 
        public ICollection<UserRole> UserRoles { get; set; } = [];
    }
}
