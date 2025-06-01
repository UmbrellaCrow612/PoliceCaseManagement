using Events;

namespace Cases.Core.Models
{
    /// <summary>
    /// Used for Attribute-Based Access Control (ABAC) or Resource-Based Access Control for <see cref="Case"/>, typically created in the system 
    /// then used in the app - system refers to the app side logic, admins do this, These are scoped to only <see cref="Case"/> 
    /// </summary>
    public class CasePermission : IDenormalizedEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// If the given user can edit details for case details - The table name itself is the permission name
        /// </summary>
        public required bool CanEdit { get; set; }

        /// <summary>
        /// If the given user can assign / edit users to the case - The table name itself is the permission name
        /// </summary>
        public required bool CanAssign { get; set; }


        /// <summary>
        /// The <see cref="Case.Id"/> it is linked to
        /// </summary>
        public required string CaseId { get; set; }
        public Case Case { get; set; } = null!;

        /// <summary>
        /// The user who has ABAC applied to them for the given case
        /// </summary>
        [DenormalizedField("Application user", "Id", "Identity Service")]
        public required string UserId { get; set; }

        [DenormalizedField("Application user", "User Name", "Identity Service")]
        public required string UserName { get; set; }
    }
}
