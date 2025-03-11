using Microsoft.AspNetCore.Identity;

namespace Identity.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Used to indicate when a password should be roated - checked on login and if so reject the login and require a new 
        /// password change - after all steps after this will be updated when there password changes so we can use it again 
        /// for the same thing
        /// </summary>
        public DateTime PasswordCreatedAt { get; set; } = DateTime.UtcNow;
        public override bool LockoutEnabled { get; set; } = true;
        public override bool TwoFactorEnabled { get; set; } = true;

        public string? DepartmentId { get; set; } = null;
        public Department? Department { get; set; } = null;
        public string? LastLoginDeviceId { get; set; } = null;

        public bool MagicLinkAuthEnabled { get; set; } = false;
        public bool OTPAuthEnabled { get; set; } = false;

        public ICollection<TwoFactorSmsAttempt> TwoFactorCodeAttempts { get; set; } = [];
        public ICollection<LoginAttempt> LoginAttempts { get; set; } = [];
        public ICollection<Token> Tokens { get; set; } = [];
        public ICollection<PasswordResetAttempt> PasswordResetAttempts { get; set; } = [];
        public ICollection<SecurityAudit> SecurityAudits { get; set; } = [];
        public ICollection<EmailVerificationAttempt> EmailVerificationAttempts { get; set; } = [];
        public ICollection<UserDevice> UserDevices { get; set; } = [];
        public ICollection<UserDeviceChallengeAttempt> UserDeviceChallengeAttempts { get; set; } = [];
        public ICollection<PhoneConfirmationAttempt> PhoneConfirmationAttempts { get; set; } = [];
        public ICollection<MagicLinkAttempt> MagicLinkAttempts { get; set; } = [];
        public ICollection<OTPAttempt> OTPAttempts { get; set; } = [];
        public ICollection<TimeBasedOneTimePassCodeBackupCode> TimeBasedOneTimePassCodeBackupCodes { get; set; } = [];
        public ICollection<PreviousPassword> PreviousPasswords { get; set; } = [];

        public TimeBasedOneTimePassCode? TimeBasedOneTimePassCode { get; set; } = null;
        public string? TimeBasedOneTimePassCodeId { get; set; } = null;
        public bool TimeBasedOneTimePassCodeEnabled { get; set; } = false;

        public bool IsTOTPAuthEnabled()
        {
            return TimeBasedOneTimePassCodeEnabled;
        }

        public bool IsOTPAuthEnabled()
        {
            return OTPAuthEnabled;
        }

        public bool IsMagicLinkAuthEnabled()
        {
            return MagicLinkAuthEnabled;
        }

        public void SetLastUsedDevice(string userDeviceId)
        {
            LastLoginDeviceId = userDeviceId;
        }

        public bool IsEmailConfirmed()
        {
            return EmailConfirmed;
        }

        public bool IsPhoneNumberConfirmed()
        {
            return PhoneNumberConfirmed;
        }

        public bool IsLinkedToADepartment()
        {
            return DepartmentId is not null;
        }

        public void LinkToDepartment(string departmentId)
        {
            DepartmentId = departmentId;
        }

        public string? LastUsedDevice()
        {
            return LastLoginDeviceId;
        }
    }
}
