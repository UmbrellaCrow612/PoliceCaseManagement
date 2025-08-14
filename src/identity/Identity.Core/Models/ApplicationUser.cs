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
        public ICollection<Device> UserDevices { get; set; } = [];


        public ICollection<PhoneVerification> PhoneVerifications { get; set; } = [];
        public ICollection<EmailVerification> EmailVerifications { get; set; } = [];
        public ICollection<DeviceVerification> DeviceVerifications { get; set; } = [];


        public bool IsEmailConfirmed()
        {
            return EmailConfirmed;
        }
    }
}
