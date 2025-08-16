namespace Identity.Core.Models
{
    /// <summary>
    /// A login attempt into the system by a user
    /// </summary>
    public class Login
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
      
        public required LoginStatus Status { get; set; }

        /// <summary>
        /// When it was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// When we consider it expired
        /// </summary>
        public required DateTime ExpiresAt { get; set; }

        /// <summary>
        /// When it used for a successful login
        /// </summary>
        public DateTime? UsedAt { get; set; } = null;


        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;


        public required string DeviceId { get; set; }
        public Device? Device { get; set; } = null;


        public ICollection<TwoFactorSms> TwoFactorSms { get; set; } = [];


        /// <summary>
        /// Checks if a login attempt is valid - considered valid if it has not been used 
        /// and has reached mfa step for use
        /// </summary>
        public bool IsValid()
        {
            if (Status != LoginStatus.TwoFactorAuthenticationReached || UsedAt.HasValue)
            {
                return false;
            }

            if (DateTime.UtcNow > ExpiresAt)
            {
                return false;
            }

            return true;
        }

        public void MarkUsed()
        {
            Status = LoginStatus.SUCCESS;
            UsedAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// All the states a login can be in
    /// </summary>
    public enum LoginStatus
    {
        /// <summary>
        /// If a login attempt was used all the way into a successful login
        /// </summary>
        SUCCESS = 0,

        /// <summary>
        /// A generic failure
        /// </summary>
        FAILED = 1,

        /// <summary>
        /// If a login attempt past the basics checks and is ready for mfa step
        /// </summary>
        TwoFactorAuthenticationReached = 3,
    }
}

