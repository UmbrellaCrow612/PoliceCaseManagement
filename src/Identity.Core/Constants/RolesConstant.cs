namespace Identity.Core.Constants
{
    /// <summary>
    /// Identity Roles Used In The System for Authorization
    /// </summary>
    public class RolesConstant
    {
        public const string Admin = "Admin";

        /// <summary>
        /// Return a read only list of roles in the system
        /// </summary>
        public static IReadOnlyCollection<string> AllRoles => [Admin];
    }
}
