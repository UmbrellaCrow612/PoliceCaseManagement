namespace CAPTCHA.Core.Models
{
    public class CAPTCHAMathQuestion
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        private const int MaxAttempts = 3;
        public double Answer {  get; set; }
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

        public bool CheckAnswer(double answer)
        {
            return Answer == answer;
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

        public bool UserEntriesExists()
        {
            return !string.IsNullOrWhiteSpace(UserAgent) && !string.IsNullOrWhiteSpace(IPAddress);
        }

        public bool Suspicious(string userAgent, string ipAddress)
        {
            return UserAgent != userAgent || IPAddress != ipAddress;
        }

        public void IncrementAttempts()
        {
            Attempts += 1;
        }

        public bool MaxAttemptLimitReached()
        {
            return Attempts >= MaxAttempts;
        }
    }
}
