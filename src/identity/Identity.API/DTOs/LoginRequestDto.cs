using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class LoginRequestDto
    {
        [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "The field cannot contain only whitespace.")]
        public string? UserName { get; set; } = null;

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
