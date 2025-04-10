using System.ComponentModel.DataAnnotations;

namespace Cases.API.DTOs
{
    public class CreateIncidentTypeDto
    {
        [Required]
        public required string Name { get; set; }

        public string? Description { get; set; } = null;
    }
}
