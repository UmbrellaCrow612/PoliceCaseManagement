using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class RegisterRequestDto
    {
        [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "The field cannot contain only whitespace.")]
        [Required]
        public required string UserName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [Phone]
        public required string PhoneNumber { get; set; }

        [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "The field cannot contain only whitespace.")]
        [Required]
        public required string Password { get; set; }
    }
}
