namespace Identity.Core.Models
{
    public class TimeBasedOneTimePassCodeBackupCode
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsUsed { get; set; } = false;
        public DateTime? UsedAt { get; set; } = null;
        public required string Code { get; set; }
        public ApplicationUser? User { get; set; } = null;
        public required string UserId { get; set; }

        public TimeBasedOneTimePassCode? TimeBasedOneTimePassCode { get; set; } = null;
        public required string TimeBasedOneTimePassCodeId { get; set; }
    }
}
