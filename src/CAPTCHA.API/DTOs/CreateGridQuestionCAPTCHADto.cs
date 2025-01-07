using System.ComponentModel.DataAnnotations;

namespace CAPTCHA.API.DTOs
{
    public class CreateGridQuestionCAPTCHADto
    {
        [Required]
        public required string Id { get; set; }

        [Required]
        [MinLength(2)]
        public ICollection<string> SelectedIds { get; set; } = [];
    }
}
