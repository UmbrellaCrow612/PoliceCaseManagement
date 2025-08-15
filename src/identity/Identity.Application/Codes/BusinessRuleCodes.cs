namespace Identity.Application.Codes
{
    /// <summary>
    /// Defines standard business rule exception codes used across the project.
    /// These codes represent various failure scenarios, including authentication, verification, and validation errors.
    /// </summary>
    public static class BusinessRuleCodes
    {
        /// <summary>
        /// Indicates that there was an issue refreshing JWT tokens for authentication.
        /// </summary>
        public const string RefreshToken = "REFRESH_TOKEN";

        /// <summary>
        /// Indicates that there was an issue with the login attempt itself - used when the login attempt dose not exist, is invalid or expired.
        /// </summary>
        public const string Login = "LOGIN";

        /// <summary>
        /// Indicates that there was a problem with the device - it was not found or is not trusted.
        /// </summary>
        public const string Device = "DEVICE";

        /// <summary>
        /// Indicates that there is a valid MFA SMS attempt already out for a login
        /// </summary>
        public const string MFASmsExists = "MFA_SMS_EXISTS";

        /// <summary>
        /// Indicates that there was a problem with authenticating a MFA step - either for example the sms code was invalid, it didn't exist or was expired
        /// </summary>

        public const string MFAInvalid = "MFA_INVALID";

        /// <summary>
        /// Indicates that a user was not found
        /// </summary>
        public const string UserNotFound = "USER_NOT_FOUND";

        /// <summary>
        /// Indicates that there was a problem with login of a user either there email or password is wrong
        /// </summary>

        public const string IncorrectCredentials = "INCORRECT_CREDENTIALS";

        /// <summary>
        /// Indicates that there was a problem sending a email to a user or by the user email as it is not yet confirmed to be real and legit
        /// </summary>

        public const string EmailNotConfirmed = "EMAIL_NOT_CONFIRMED";

        /// <summary>
        /// Indicates that there was a problem sending a SMS to a user or by the user phone number as it is not yet confirmed to be real and legit
        /// </summary>

        public const string PhoneNotConfirmed = "PHONE_NOT_CONFIRMED";
    }
}