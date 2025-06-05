using Events;
using Events.Core;

namespace Cases.Core.Models
{
    /// <summary>
    /// Used for Attribute-Based Access Control (ABAC) or Resource-Based Access Control for <see cref="Case"/>, the table name themselves \
    /// are the permission names issued out if they are flagged on and the checked on endpoints if needed
    /// </summary>
    public class CasePermission : IDenormalizedEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// If the given user can edit details for case details - The table name itself is the permission name
        /// </summary>
        public required bool CanEdit { get; set; }



        /// <summary>
        /// If a user can see the permissions set on the case
        /// </summary>
        public required bool CanViewPermissions { get; set; }

        /// <summary>
        /// If a user can edit permissions on a case
        /// </summary>
        public required bool CanEditPermissions { get; set; }



        /// <summary>
        /// If a user can see what files are attached to a case
        /// </summary>
        public required bool CanViewFileAttachments { get; set; }

        /// <summary>
        /// If a user can remove file attachments
        /// </summary>
        public required bool CanDeleteFileAttachments { get; set; }



        /// <summary>
        /// If a user can see who is linked to a given case
        /// </summary>
        public required bool CanViewAssigned { get; set; }

        /// <summary>
        /// If the given user can assign users to the case - The table name itself is the permission name
        /// </summary>
        public required bool CanAssign { get; set; }

        /// <summary>
        /// If a user can remove assigned users
        /// </summary>
        public required bool CanRemoveAssigned { get; set; }



        /// <summary>
        /// If the user can see actions performed on the case - The table name itself is the permission name
        /// </summary>
        public required bool CanViewActions { get; set; }

        /// <summary>
        /// If a user can add a case case action to a case - The table name itself is the permission name
        /// </summary>
        public required bool CanAddActions { get; set; }

        /// <summary>
        /// If a user can edit actions taken on a case - The table name itself is the permission name
        /// </summary>
        public required bool CanEditActions { get; set; }

        /// <summary>
        /// If a user can delete actions taken on a case - The table name itself is the permission name
        /// </summary>
        public required bool CanDeleteActions { get; set; }


        /// <summary>
        /// By default they can view incident types on a case - but they need perms to edit or remove them from given case - this
        /// if turned on makes it so they can edit and remove <see cref="IncidentType"/> linked to a given case, NOTE: this should not be used to edit 
        /// <see cref="IncidentType"/> model itself only to allow them to link or unlink them
        /// </summary>
        public required bool CanEditIncidentType { get; set; }


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
