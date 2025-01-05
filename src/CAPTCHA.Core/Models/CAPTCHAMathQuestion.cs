namespace CAPTCHA.Core.Models
{
    public class CAPTCHAMathQuestion
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Answer {  get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public required DateTime ExpiresAt {  get; set; }
        public bool IsSuccessful { get; set; } = false;
        public DateTime? SuccessfulAt { get; set; } = null;
        public string? IPAddress { get; set; } = null;
        public int Attempts { get; set; } = 0;
        public string? UserAgent { get; set; } = null;

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

        public void MarkAsSuccessful()
        {
            SuccessfulAt = DateTime.UtcNow;
            IsSuccessful = true;
        }

        public void SetUser(string userAgent, string ipAddress)
        {
            UserAgent = userAgent;
            IPAddress = ipAddress;
        }

        public void IncrementAttempts()
        {
            Attempts += 1;
        }
    }

    // The answer will be a math expression hshed btypes sent across with the ID of the question, we wont store that part
    // Then that ill be decrptyed on frontnend and displayed
    // thy wuld sed {idOfCaptcha: 123, answer: 123}
}
