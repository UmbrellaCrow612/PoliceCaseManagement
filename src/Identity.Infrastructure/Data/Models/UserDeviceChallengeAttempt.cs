namespace Identity.Infrastructure.Data.Models
{
    public class UserDeviceChallengeAttempt
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Email { get; set; }
        public required string Code { get; set; }
        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;
        public required string UserDeviceId { get; set; }
        public UserDevice? UserDevice { get; set; } = null;
        public bool IsSuccessful { get; set; } = false;
        public DateTime? SuccessfulAt { get; set; } = null;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsValid(double windowTime = 60)
        {
            return !IsSuccessful && DateTime.UtcNow < CreatedAt.AddMinutes(windowTime);
        }

        public void MarkUsed()
        {
            IsSuccessful = true;
            SuccessfulAt = DateTime.UtcNow;
        }
    }
}
