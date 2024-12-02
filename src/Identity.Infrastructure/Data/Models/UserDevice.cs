namespace Identity.Infrastructure.Data.Models
{
    public class UserDevice
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;
        public required string DeviceName { get; set; }
        public required string DeviceIdentifier { get; set; } // put an index on this - make way for no conflicts
        public required bool IsTrusted { get; set; } // flag and block those not trusted - send alerts or this
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<UserDeviceChallengeAttempt> UserDeviceChallengeAttempts { get; set; } = [];
    }
}
