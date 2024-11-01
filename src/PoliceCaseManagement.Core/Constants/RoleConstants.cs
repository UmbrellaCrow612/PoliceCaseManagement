namespace PoliceCaseManagement.Core.Constants
{
    public class RoleConstants
    {
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string User = "User";
        public const string Guest = "Guest";

        public string[] Roles { get; set; } = { Admin, Manager, User, Guest };
    }
}
