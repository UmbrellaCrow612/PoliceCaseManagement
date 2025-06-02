namespace Identity.Core.Models
{
    public class TwoFactorEmailAttempt
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Email { get; set; }
        public required string Code { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsSuccessful { get; set; } = false;
        public DateTime? SuccessfulAt { get; set; } = null;
        public required string LoginAttemptId { get; set; }
        public LoginAttempt? LoginAttempt { get; set; } = null;
        public required DateTime ExpiresAt { get; set; }

        public bool IsValid()
        {
            if (IsSuccessful)
            {
                return false; // already been used
            }

            if (DateTime.UtcNow > ExpiresAt)
            {
                return false; // time elapsed
            }

            return true;
        }

        public void MarkUsed()
        {
            IsSuccessful = true;
            SuccessfulAt = DateTime.UtcNow;
        }
    }
}
