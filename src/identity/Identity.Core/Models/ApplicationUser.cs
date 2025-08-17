using Microsoft.AspNetCore.Identity;

namespace Identity.Core.Models
{
    /// <summary>
    /// Model for user's in the system
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public override bool LockoutEnabled { get; set; } = true;
        public override bool TwoFactorEnabled { get; set; } = true;


        /// <summary>
        /// TOTP (Time Based One Time Passcodes) secret field - used as part of totp to generate a totp code 
        /// </summary>
        public string? TotpSecret { get; set; } = null;

        /// <summary>
        /// Flag to indicate to if TOTP (Time Based One Time Passcodes) has been set up and verified and confirmed to be turned on
        /// </summary>
        public bool TotpConfirmed { get; set; } = false;


        public ICollection<TwoFactorSms> TwoFactorSms { get; set; } = [];



        public ICollection<Login> Logins { get; set; } = [];
        public ICollection<Token> Tokens { get; set; } = [];
        public ICollection<Device> Devices { get; set; } = [];


        public ICollection<PhoneVerification> PhoneVerifications { get; set; } = [];
        public ICollection<EmailVerification> EmailVerifications { get; set; } = [];
        public ICollection<DeviceVerification> DeviceVerifications { get; set; } = [];


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
