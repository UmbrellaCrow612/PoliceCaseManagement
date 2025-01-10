namespace Identity.Core.Models
{
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
    }
}
