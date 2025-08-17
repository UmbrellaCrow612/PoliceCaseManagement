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
    }
}
