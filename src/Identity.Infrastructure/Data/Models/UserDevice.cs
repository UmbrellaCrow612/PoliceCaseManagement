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

        /// <summary>
        /// Last success login
        /// </summary>
        public DateTime? LastLogin { get; set; } = null;

        /// <summary>
        /// Made from this device.
        /// </summary>
        public ICollection<LoginAttempt> LoginAttempts { get; set; } = [];

        /// <summary>
        /// Tokens issued to this device.
        /// </summary>
        public ICollection<Token> Tokens { get; set; } = [];
    }
}
