namespace Identity.Core.Constants
{
    /// <summary>
    /// Identity Roles Used In The System for Authorization
    /// </summary>
    public class RolesConstant
    {
        public const string Admin = "Admin";

        public static IReadOnlyCollection<string> AllRoles => [Admin];
    }
}
