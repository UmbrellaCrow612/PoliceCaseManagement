using System.ComponentModel.DataAnnotations;

namespace CAPTCHA.API.DTOs
{
    public class CreateMathQuestionCAPTCHADto
    {
        [Required]
        public required string MathQuestionId { get; set; }

        [Required]
        public required string Answer {  get; set; }
    }
}
