namespace CAPTCHA.Core.Models
{
    public class CAPTCHAGridParentQuestion
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string ValueInPlainText { get; set; }
        public ICollection<CAPTCHAGridChild> Children { get; set; } = [];
    }

    public class CAPTCHAGridChild
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string ValueInPlainText { get; set; }

        public required string CAPTCHAGridParentQuestionId { get; set; }
        public CAPTCHAGridParentQuestion? question { get; set; } = null;
    }
}
