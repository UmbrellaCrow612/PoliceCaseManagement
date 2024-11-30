namespace Identity.Infrastructure.Data.Models
{
    public class EmailVerificationAttempt
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;
        public required string Email {  get; set; }
        public required string Code { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public required DateTime ExpiresAt {  get; set; }
        public DateTime? UsedAt { get; set; }
        public required bool IsUsed { get; set; } = false;

        /// <summary>
        /// Returns true if, it is not used or elapsed the allowed time.
        /// </summary>
        /// <param name="allowedTimeWindow">How long you want to allow a attempt to be valid for.</param>
        public bool IsValid(double allowedTimeWindow = 60)
        {
            return !IsUsed && DateTime.UtcNow <= ExpiresAt.AddMinutes(allowedTimeWindow);
        }

        /// <summary>
        /// Sets the used and used at propertys to true and now
        /// </summary>
        public void MarkUsed()
        {
            IsUsed = true;
            UsedAt = DateTime.UtcNow;
        }
    }
}
