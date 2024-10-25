using System.ComponentModel.DataAnnotations;

namespace PoliceCaseManagement.Application.DTOs.Cases
{
    public class UpdateCaseDto
    {
        [Required]
        public required string Title { get; set; }
        [Required]
        public required string Description { get; set; }
    }
}
