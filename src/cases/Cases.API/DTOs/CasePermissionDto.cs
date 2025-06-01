namespace Cases.API.DTOs
{
    public class CasePermissionDto
    {
        public required string Id { get; set; }

        public required bool CanEdit { get; set; }

        public required bool CanAssign { get; set; }

        public required string CaseId { get; set; }

        public required string UserId { get; set; }

        public required string UserName { get; set; }
    }
}
