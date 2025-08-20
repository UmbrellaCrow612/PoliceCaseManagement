namespace Identity.Core.Models.Joins
{
    /// <summary>
    /// Join between a many to many relation between a <see cref="ApplicationUser"/> and <see cref="ApplicationRole"/>
    /// </summary>
    public class UserRole
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// the ID of the user
        /// </summary>
        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;

        /// <summary>
        /// the ID of the role to link to
        /// </summary>
        public required string RoleId { get; set; }
        public ApplicationRole? Role { get; set; } = null;
    }

}
