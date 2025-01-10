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

        /// <summary>
        /// Checks if a attempt is valid - By checking if it not already used or expired
        /// </summary>
        /// <param name="windowTime">The time a attempt is considred valid for</param>
        public bool IsValid(double windowTime)
        {
            if (IsSuccessful)
            {
                return false; // already been used
            }

            if (DateTime.UtcNow > CreatedAt.AddMinutes(windowTime))
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
