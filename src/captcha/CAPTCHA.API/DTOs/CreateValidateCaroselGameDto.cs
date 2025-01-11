namespace CAPTCHA.API.DTOs
{
    public class CreateValidateCaroselGameDto
    {
        public required string Id { get; set; }
        public ICollection<ChoiceDto> Choices { get; set; } = [];
    }

    public class ChoiceDto
    {
        public required string Id { get; set; }

        public ICollection<string> SelectedIds { get; set; } = []
    }
}
