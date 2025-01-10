using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class RefreshTokenRequestDto
    {
        [Required]
        public required string RefreshToken { get; set; }
    }
}
