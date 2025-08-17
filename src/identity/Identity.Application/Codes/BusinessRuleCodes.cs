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
        /// Indicates that there was a problem creating a device - used when trying ot create a device that already exists
        /// </summary>
        public const string DeviceExists = "DEVICE_EXISTS";

        /// <summary>
        /// Indicates that a device is already trusted - used when trying to re verify it 
        /// </summary>
        public const string DeviceAlreadyTrusted = "DEVICE_ALREADY_TRUSTED";

        /// <summary>
        /// Indicates that a valid device verification exists - used when trying to send new ones back to back whilst the previous one is valid and not yet expired 
        /// </summary>
        public const string DeviceVerificationExists = "DEVICE_VERIFICATION_EXISTS";

        /// <summary>
        /// Indicates that a valid device verification is invalid - used when attempt dose not exist for verification attempt or the attempt is invalid or expired
        /// </summary>
        public const string DeviceVerificationInvalid = "DEVICE_VERIFICATION_INVALID";

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

        /// <summary>
        /// Indicates that the username is taken by another user - usernames are unique 
        /// </summary>

        public const string UserNameTaken = "USERNAME_TAKEN";

        /// <summary>
        /// Indicates that the email is taken by another user - emails are unique 
        /// </summary>

        public const string UserEmailTaken = "USER_EMAIL_TAKEN";

        /// <summary>
        /// Indicates that the phone number is taken by another user - phone numbers are unique 
        /// </summary>

        public const string UserPhoneNumberTaken = "USER_PHONENUMBER_TAKEN";

        /// <summary>
        /// Indicates that a email is already verified - used when trying to verify a email that is already verified
        /// </summary>

        public const string EmailVerified = "EMAILL_VERIFIED";

        /// <summary>
        /// Indicates that there is a valid email verification - used when trying to send a email verification when a valid one already exists
        /// </summary>
        public const string EmailVerificationExists = "EMAIL_VERIFICATION_EXISTS";

        /// <summary>
        /// Indicates that a email verification is invalid = used when the email verification was not found or invalid or expired
        /// </summary>
        public const string EmailVerificationInvalid = "EMAIL_VERIFICATION_INVALID";

        /// <summary>
        /// Indicates that a phone number is already verified
        /// </summary>

        public const string PhoneNumberVerified = "PHONE_NUMBER_VERIFIED";

        /// <summary>
        /// Indicates that a phone verification exists - used when a valid verification exists while trying to send a new one 
        /// </summary>

        public const string PhoneNumberVerificationExists = "PHONE_NUMBER_VERIFICATION_EXISTS";

        /// <summary>
        /// Indicates that a phone verification is invalid - used when it is expired, used or not found 
        /// </summary>

        public const string PhoneNumberVerificationInvalid = "PHONE_NUMBER_VERIFICATION_INVALID";

        /// <summary>
        /// Indicates that a user has TOTP (Time Based One Time Passcode) has already been created for a user = used when trying to turn on TOTP for a user who already
        /// has it 
        /// </summary>

        public const string TOTPExists = "TOTP_EXISTS";

        /// <summary>
        /// Indicates that a user dose not have TOTP (Time Based One Time Passcodes) - used when trying to reset a TOTP
        /// </summary>

        public const string TOTPReset = "TOTP_RESET";
    }
}