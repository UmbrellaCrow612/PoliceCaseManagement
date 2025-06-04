namespace Cases.API.DTOs
{
    public class UpdateCasePermissionDto
    {
        public required string Id { get; set; }

        public required bool CanEdit { get; set; }

        public required bool CanViewPermissions { get; set; }

        public required bool CanEditPermissions { get; set; }

        public required bool CanViewFileAttachments { get; set; }

        public required bool CanDeleteFileAttachments { get; set; }

        public required bool CanViewAssigned { get; set; }

        public required bool CanAssign { get; set; }

        public required bool CanRemoveAssigned { get; set; }

        public required bool CanViewActions { get; set; }

        public required bool CanAddActions { get; set; }

        public required bool CanEditActions { get; set; }

        public required bool CanDeleteActions { get; set; }

        public required bool CanEditIncidentType { get; set; }
    }
}
