using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data.Models
{
    [Index(nameof(PhoneNumber), nameof(Code))]
    public class TwoFactorCodeAttempt
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
            return !IsSuccessful && CreatedAt.AddMinutes(windowTime) > DateTime.UtcNow;
        }

        public void MarkUsed()
        {
            IsSuccessful = true;
            SuccessfulAt = DateTime.UtcNow;
        }
    }
}
