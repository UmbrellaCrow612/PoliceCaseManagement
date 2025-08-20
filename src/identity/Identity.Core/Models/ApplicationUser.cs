using Identity.Core.Models.Joins;

namespace Identity.Core.Models
{
    /// <summary>
    /// Model for user's in the system
    /// </summary>
    public class ApplicationUser
    {
        /// <summary>
        /// The ID of the current user
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// The current users username
        /// </summary>
        public required string UserName { get; set; }

        /// <summary>
        /// The current users email address
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// The current users Phone number
        /// </summary>
        public required string PhoneNumber { get; set; }

        /// <summary>
        /// Flag to indicate if the current user has there phone number confirmed
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; } = false;

        /// <summary>
        /// Flag to indicate if the current user had there email confirmed
        /// </summary>
        public bool EmailConfirmed { get; set; } = false;

        /// <summary>
        /// TOTP (Time Based One Time Passcodes) secret field - used as part of totp to generate a totp code 
        /// </summary>
        public string? TotpSecret { get; set; } = null;

        /// <summary>
        /// Flag to indicate to if TOTP (Time Based One Time Passcodes) has been set up and verified and confirmed to be turned on
        /// </summary>
        public bool TotpConfirmed { get; set; } = false;

        /// <summary>
        /// The hashed password of the current user
        /// </summary>
        public string PasswordHash { get; set; } = null!;



        // Ef core relations 
        public ICollection<TwoFactorSms> TwoFactorSms { get; set; } = [];
        public ICollection<Login> Logins { get; set; } = [];
        public ICollection<Token> Tokens { get; set; } = [];
        public ICollection<Device> Devices { get; set; } = [];
        public ICollection<PhoneVerification> PhoneVerifications { get; set; } = [];
        public ICollection<EmailVerification> EmailVerifications { get; set; } = [];
        public ICollection<DeviceVerification> DeviceVerifications { get; set; } = [];
        public ICollection<UserRole> UserRoles { get; set; } = [];




        /// <summary>
        /// Marks a user's email as confirmed
        /// </summary>
        public void MarkEmailConfirmed()
        {
            EmailConfirmed = true;
        }

        public void MarkPhoneNumberConfirmed()
        {
            PhoneNumberConfirmed = true;
        }

        public void ResetTotp()
        {
            TotpConfirmed = false;
            TotpSecret = null;
        }

        public void MarkTotpConfirmed()
        {
            TotpConfirmed = true;
        }
    }
}
