using PoliceCaseManagement.Core.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace PoliceCaseManagement.Application.DTOs.Cases
{
    public class CreateCaseDto
    {
        [Required]
        [EnumDataType(typeof(CasePriority))]
        public required CasePriority CasePriority { get; set; }
        [Required]
        public required string Title { get; set; }
        [Required]
        public required string Description { get; set; }
    }
}
