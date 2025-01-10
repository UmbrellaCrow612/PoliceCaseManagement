namespace CAPTCHA.Core.Models
{
    public class CAPTCHAAudioQuestion
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string AnswerInPlainText { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public required DateTime ExpiresAt { get; set; }
        public bool IsSuccessful { get; set; } = false;
        public DateTime? SuccessfulAt { get; set; } = null;
        public int Attempts { get; set; } = 0;
        private int MaxAttempts { get; set; } = 3;

        public void IncrementAttempts()
        {
            Attempts += 1;
        }

        public bool MaxAttemptLimitReached()
        {
            return Attempts >= MaxAttempts;
        }

        public bool IsValid()
        {
            if (DateTime.UtcNow > ExpiresAt)
            {
                return false;
            }

            if (IsSuccessful)
            {
                return false;
            }

            return true;
        }

        public bool CheckAnswer(string answer)
        {
            return string.Equals(AnswerInPlainText, answer);
        }

        public void MarkAsSuccessful()
        {
            SuccessfulAt = DateTime.UtcNow;
            IsSuccessful = true;
        }
    }
}
