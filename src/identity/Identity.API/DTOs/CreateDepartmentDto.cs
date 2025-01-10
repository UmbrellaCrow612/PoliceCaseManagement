using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class CreateDepartmentDto
    {
        [Required]
        public required string Name { get; set; }
    }
}
