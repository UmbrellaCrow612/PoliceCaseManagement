using System.ComponentModel.DataAnnotations;

namespace PoliceCaseManagement.Application.DTOs.Roles
{
    public class CreateRoleDto
    {
        [Required]
        public required string Name { get; set; }
    }
}
