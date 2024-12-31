namespace Identity.Core.Models
{
    public class TwoFactorSmsAttempt
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsSuccessful { get; set; } = false;
        public DateTime? SuccessfulAt { get; set; } = null;
        public required string LoginAttemptId { get; set; }
        public LoginAttempt? LoginAttempt { get; set; } = null;
        public required string Code { get; set; }
        public required string PhoneNumber { get; set; }
        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;

        public bool IsValid(double windowTime)
        {
            if (IsSuccessful is true)
            {
                return false;
            }

            if (DateTime.UtcNow > CreatedAt.AddMinutes(windowTime))
            {
                return false;
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
