using System.Runtime.Serialization;

namespace Identity.Core.Models
{
    public class EmailVerificationAttempt
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;
        public required string Email { get; set; }
        public required string Code { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UsedAt { get; set; }
        public bool IsSuccessful { get; set; } = false;

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
            UsedAt = DateTime.UtcNow;
        }
    }
}
