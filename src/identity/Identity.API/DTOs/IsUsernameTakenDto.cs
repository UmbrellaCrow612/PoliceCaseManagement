using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class IsUsernameTakenDto
    {
        [Required]
        public required string Username { get; set; }
    }
}
