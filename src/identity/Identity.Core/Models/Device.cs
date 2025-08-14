namespace Identity.Core.Models
{
    /// <summary>
    /// Represents a <see cref="ApplicationUser"/> device used to login in with
    /// </summary>
    public class Device
    {
        /// <summary>
        /// A ID generated using hash algorithm based on the <see cref="ValueObjects.DeviceInfo"/> and <see cref="ApplicationUser"/>
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// A name to identify it
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// If the device is trusted
        /// </summary>
        public bool IsTrusted { get; set; } = false;

        /// <summary>
        /// When it was added
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;



        public ICollection<DeviceVerification> DeviceVerifications { get; set; } = [];

    }
}
