namespace Identity.Core.Constants
{
    public class PermissionsConstant
    {
        public const string Admin = "Admin";

        public static IReadOnlyCollection<string> AllPermissions => [Admin];
    }
}
