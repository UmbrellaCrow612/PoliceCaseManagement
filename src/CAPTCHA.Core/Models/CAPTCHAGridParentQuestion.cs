namespace CAPTCHA.Core.Models
{
    public class CAPTCHAGridParentQuestion
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public ICollection<CAPTCHAGridChild> Children { get; set; } = [];

        public required DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; } = false;
        public DateTime? UsedAt { get; set; } = null;


        public bool IsValid(ICollection<string> selectedIds)
        {
            if (DateTime.UtcNow > ExpiresAt || IsUsed || UsedAt.HasValue)
            {
                return false;
            }

            var childIds = Children.Select(child => child.Id).ToList();
            if (!selectedIds.All(id => childIds.Contains(id)))
            {
                return false;
            }

            return true;
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
        public CAPTCHAGridParentQuestion? Question { get; set; } = null;
    }
}
