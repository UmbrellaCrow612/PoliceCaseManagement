namespace Identity.Core.Models
{
    public class UserDevice
    {

        public required string Id { get; set; }
        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;
        public required string DeviceName { get; set; }
        public bool IsTrusted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<UserDeviceChallengeAttempt> UserDeviceChallengeAttempts { get; set; } = [];

        public void MarkTrusted()
        {
            IsTrusted = true;
        }

        public bool Trusted()
        {
            return IsTrusted;
        }

    }
}
