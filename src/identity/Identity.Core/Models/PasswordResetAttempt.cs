namespace Identity.Core.Models
{
    public class PasswordResetAttempt
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;
        public required string Code { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? SuccessfulAt { get; set; }
        public bool IsSuccessful { get; set; } = false;
        public bool IsRevoked { get; set; } = false;

        public bool IsValid(double windowTime)
        {
            if (IsRevoked)
            {
                return false;
            }

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

        public void Revoke()
        {
            IsRevoked = true;
        }
    }
}
