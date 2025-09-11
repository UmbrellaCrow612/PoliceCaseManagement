using System.Text.RegularExpressions;

namespace Identity.Core.RegexHelpers
{
    /// <summary>
    /// Contains all our application regular expressions used for <see cref="Models.ApplicationUser"/> validation of data
    /// </summary>
    public static partial class UserRegex
    {
        [GeneratedRegex(@"^[a-zA-Z0-9_\-@!]{3,32}$")]
        public static partial Regex UsernameRegex();

        [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
        public static partial Regex EmailRegex();

        [GeneratedRegex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$")]
        public static partial Regex PasswordRegex();

        [GeneratedRegex(@"^\+[1-9]\d{7,14}$")]
        public static partial Regex PhoneNumberRegex();
    }
}
