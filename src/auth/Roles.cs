namespace Auth
{
    /// <summary>
    /// Authorization roles used throughout the system 
    /// </summary>
    public class Roles
    {
        public const string Admin = "Admin";

        /// <summary>
        /// Return a read only list of roles in the system
        /// </summary>
        public static IReadOnlyCollection<string> AllRoles => [Admin];
    }
}
