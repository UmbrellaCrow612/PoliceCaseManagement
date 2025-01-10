namespace CAPTCHA.Core.Models
{
    public class CAPTCHAGridQuestion
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public ICollection<CAPTCHAGridChild> Children { get; set; } = [];

        public required DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; } = false;
        public DateTime? UsedAt { get; set; } = null;


        public (bool isValid, ICollection<string> errors) IsValid(ICollection<string> selectedIds)
        {
            List<string> errs = [];

            if (DateTime.UtcNow > ExpiresAt) errs.Add("Question has expired.");
            if (IsUsed || UsedAt.HasValue) errs.Add("Question is already used.");

            var childIds = Children.Select(c => c.Id).ToList();
            bool areEqual = selectedIds.OrderBy(x => x).SequenceEqual(childIds.OrderBy(x => x));

            if (!areEqual)
            {
                errs.Add("Not all selected IDs match or exist for the question.");
            }

            if (errs.Count != 0) return (false, errs);

            return (true, errs);
        }

        public void MarkUsed()
        {
            UsedAt = DateTime.UtcNow;
            IsUsed = true;
        }

    }

    public class CAPTCHAGridChild
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();


        private byte[] _tempBytes = [];
        public void SetTempBytes(byte[] bytes)
        {
            _tempBytes = bytes;
        }

        public byte[] GetTempBytes()
        {
            return _tempBytes;
        }

        public required string CAPTCHAGridParentQuestionId { get; set; }
        public CAPTCHAGridQuestion? Question { get; set; } = null;
    }
}
