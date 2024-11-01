namespace PoliceCaseManagement.Core.Constants
{
    /// <summary>
    /// Roles in the Police Case Management System
    /// </summary>
    public static class RoleConstants
    {
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string User = "User";
        public const string Guest = "Guest";

        public static string[] Roles { get; set; } = [Admin, Manager, User, Guest];
    }
}
