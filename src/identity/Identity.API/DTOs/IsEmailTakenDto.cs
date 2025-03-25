using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class IsEmailTakenDto
    {
        [Required]
        public required string Email { get; set; }
    }
}
