using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class LoginRequestDto
    {
        [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "The field cannot contain only whitespace.")]
        public string? UserName { get; set; } = null;

        [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "The field cannot contain only whitespace.")]
        [EmailAddress]
        public string? Email { get; set; } = null;

        [Required]
        public required string Password { get; set; }
    }
}
