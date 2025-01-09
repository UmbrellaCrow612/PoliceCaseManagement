namespace CAPTCHA.Core.Models
{
    public class CAPTCHACarouselChoiceGameQuestion
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public ICollection<CarouselChoiceChild> Choices { get; set; } = [];
    }

    public class CarouselChoiceChild
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        private string TempAnswer { get; set; } = "";
        public void SetTempAnswer(string answer)
        {
            TempAnswer = answer;
        }

        public string GetTempAnswer()
        {
            return TempAnswer;
        }

        public CAPTCHACarouselChoiceGameQuestion? Question { get; set; } = null;
        public required string CaptchaCarouselChoiceGameQuestionId { get; set; }

        public ICollection<SubCarouselChoiceChild> SubChoices { get; set;} = [];
        private ICollection<SubCarouselChoiceChild> TempFullSubChoices { get; set; } = [];

        public void SetTempFullSubChoice(SubCarouselChoiceChild subChild)
        {
            TempFullSubChoices.Add(subChild);
        }

        public ICollection<SubCarouselChoiceChild> GetTempFullSubChoices()
        {
            return TempFullSubChoices;
        }


        public bool IsValid(ICollection<string> selectedChoices)
        {
            return false;
        }
    }

    public class SubCarouselChoiceChild
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public CarouselChoiceChild? CarouselChoiceChild { get; set; } = null;
        public required string CarouselChoiceChildId { get; set; }


        private byte[] _tempBytes = [];
        public void SetTempBytes(byte[] bytes)
        {
            _tempBytes = bytes;
        }

        public byte[] GetTempBytes()
        {
            return _tempBytes;
        }
    }
}
