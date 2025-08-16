using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class EmailTakenDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
