namespace Identity.Core.Constants
{
    /// <summary>
    /// Core Identity InternalRoles
    /// </summary>
    public static class InternalRoles
    {
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string User = "User";

        /// <summary>
        /// Access all internal all defined
        /// </summary>
        public static readonly IReadOnlyCollection<string> All =
        [
            Admin,
            Manager,
            User
        ];
    }
}
