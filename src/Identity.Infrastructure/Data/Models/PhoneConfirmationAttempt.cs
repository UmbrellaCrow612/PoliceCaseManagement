using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data.Models
{
    [Index(nameof(UserId))]
    [Index(nameof(PhoneNumber))]
    public class PhoneConfirmationAttempt
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Code { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;
        public required string PhoneNumber { get; set; }
        public bool IsSuccessful { get; set; } = false;
        public DateTime? SuccessfulAt { get; set; } = null;

        public void MarkUsed()
        {
            IsSuccessful = true;
            SuccessfulAt = DateTime.UtcNow;
        }

        public bool IsValid(double windowTime = 60)
        {
            return !IsSuccessful && DateTime.UtcNow < CreatedAt.AddMinutes(windowTime);
        }
    }
}
