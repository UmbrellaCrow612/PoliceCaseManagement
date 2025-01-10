using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class UpdateDepartmentDto
    {
        [Required]
        public required string Name { get; set; }
    }
}
