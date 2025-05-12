using Cases.Core.Models;

namespace Cases.API.DTOs
{
    public class CaseActionDto
    {
        public required string Id { get; set; }
        public required string Description { get; set; }
        public required string? Notes { get; set; }
        public required CaseActionValidationStatus ValidationStatus { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required string CreatedById { get; set; }
    }
}
