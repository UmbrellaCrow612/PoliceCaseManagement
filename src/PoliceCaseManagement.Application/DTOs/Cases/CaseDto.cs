using PoliceCaseManagement.Core.Entities.Enums;

namespace PoliceCaseManagement.Application.DTOs.Cases
{
    public class CaseDto
    {
        public required string Id { get; set; }
        public required CasePriority CasePriority { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string CreatedById { get; set; }
    }
}
