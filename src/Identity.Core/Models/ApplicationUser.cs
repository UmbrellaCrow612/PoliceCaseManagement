using Microsoft.AspNetCore.Identity;

namespace Identity.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public override bool LockoutEnabled { get; set; } = true;
        public override bool TwoFactorEnabled { get; set; } = true;

        public string? DepartmentId { get; set; } = null;
        public Department? Department { get; set; } = null;
        public string? LastLoginDeviceId { get; set; } = null;

        public ICollection<TwoFactorSmsAttempt> TwoFactorCodeAttempts { get; set; } = [];
        public ICollection<LoginAttempt> LoginAttempts { get; set; } = [];
        public ICollection<Token> Tokens { get; set; } = [];
        public ICollection<PasswordResetAttempt> PasswordResetAttempts { get; set; } = [];
        public ICollection<SecurityAudit> SecurityAudits { get; set; } = [];
        public ICollection<EmailVerificationAttempt> EmailVerificationAttempts { get; set; } = [];
        public ICollection<UserDevice> UserDevices { get; set; } = [];
        public ICollection<UserDeviceChallengeAttempt> UserDeviceChallengeAttempts { get; set; } = [];
        public ICollection<PhoneConfirmationAttempt> PhoneConfirmationAttempts { get; set; } = [];

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
