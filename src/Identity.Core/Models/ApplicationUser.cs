using Microsoft.AspNet.Identity.EntityFramework;

namespace Identity.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public override bool LockoutEnabled { get; set; } = true;
        public override bool TwoFactorEnabled { get; set; } = true;

        public string? DepartmentId { get; set; } = null;
        public string? LastLoginDeviceId { get; set; } = null;

        public bool IsLinkedToADepartment()
        {
            return DepartmentId is null;
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
