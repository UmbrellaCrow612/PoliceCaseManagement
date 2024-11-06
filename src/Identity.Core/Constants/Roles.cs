namespace Identity.Core.Constants
{
    /// <summary>
    /// Core Identity Roles
    /// </summary>
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string User = "User";

        public static readonly IReadOnlyCollection<string> All =
        [
            Admin,
            Manager,
            User
        ];
    }
}
