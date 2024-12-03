namespace Identity.Infrastructure.Data.Models
{
    public class UserDevice
    {
        public required string Id { get; set; }
        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;
        public required string DeviceName { get; set; }
        public required bool IsTrusted { get; set; } // flag and block those not trusted - send alerts or this
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<UserDeviceChallengeAttempt> UserDeviceChallengeAttempts { get; set; } = [];
    }
}
