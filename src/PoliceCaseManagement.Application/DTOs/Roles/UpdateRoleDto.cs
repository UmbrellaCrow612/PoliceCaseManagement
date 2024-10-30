using System.ComponentModel.DataAnnotations;

namespace PoliceCaseManagement.Application.DTOs.Roles
{
    public class UpdateRoleDto
    {
        [Required]
        public required string Name { get; set; }
    }
}
