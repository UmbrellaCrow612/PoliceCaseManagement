using System.ComponentModel.DataAnnotations;

namespace Cases.API.DTOs
{
    public class CreateCaseDto
    {
        public string? CaseNumber { get; set; } = null;
        public string? Summary { get; set; } = null;
        public string? Description { get; set; } = null;

        [Required]
        public required DateTime IncidentDateTime { get; set; }
    }
}
