namespace Cases.API.DTOs
{
    public class UpdateCasePermissionDto
    {
        public required string Id { get; set; }

        public required bool CanEdit { get; set; }

        public required bool CanAssign { get; set; }
    }
}
