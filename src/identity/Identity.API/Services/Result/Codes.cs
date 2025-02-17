namespace Identity.API.Services.Result
{
    /// <summary>
    /// Defines standard business rule exception codes used across the project in <see cref="IServiceError.Code"/>.
    /// These codes represent various failure scenarios, including authentication, validation, and general errors.
    /// </summary>
    public static class Codes
    {
        /// <summary>
        /// Indicates that the provided credentials are incorrect.
        /// </summary>
        public static readonly string IncorrectCreds = "INCORRECT_CREDENTIALS";

        /// <summary>
        /// Indicates that the account has been locked due to security policies or excessive failed attempts.
        /// </summary>
        public static readonly string AccountLocked = "ACCOUNT_LOCKED";

        /// <summary>
        /// Indicates that the user's email address has not been confirmed.
        /// </summary>
        public static readonly string EmailNotConfirmed = "EMAIL_NOT_CONFIRMED";

        /// <summary>
        /// Indicates that the user's phone number has not been confirmed.
        /// </summary>
        public static readonly string PhoneNumberNotConfirmed = "PHONE_NOT_CONFIRMED";

        /// <summary>
        /// Indicates that the device used for an operation has not been confirmed.
        /// </summary>
        public static readonly string DeviceNotConfirmed = "DEVICE_NOT_CONFIRMED";

        /// <summary>
        /// Represents an email confirmation process or token-related issue.
        /// </summary>
        public static readonly string EmailConfirmation = "EMAIL_CONFIRMATION";

        /// <summary>
        /// Indicates that a validation error occurred, typically due to invalid input data.
        /// </summary>
        public static readonly string ValidationError = "VALIDATION_ERROR";

        /// <summary>
        /// Indicates that the specified user does not exist.
        /// </summary>
        public static readonly string UserDoesNotExist = "USER_DOES_NOT_EXIST";

        /// <summary>
        /// Indicates that the attempted operation is not valid.
        /// </summary>
        public static readonly string LoginAttemptNotValid = "LOGIN_ATTEMPT_NOT_VALID";

        /// <summary>
        /// Indicates that the email address has already been confirmed and does not require further verification.
        /// </summary>
        public static readonly string EmailAlreadyConfirmed = "EMAIL_ALREADY_CONFIRMED";

        /// <summary>
        /// Indicates that a valid two-factor authentication attempt via email confirmation already exists.
        /// </summary>
        public static readonly string ValidTwoFactorEmailAttemptExists = "VALID_TWO_FACTOR_EMAIL_CONFIRMATION_ALREADY_EXISTS";

        /// <summary>
        /// Indicates that a valid two-factor authentication attempt via SMS already exists.
        /// </summary>
        public static readonly string ValidTwoFactorSmsAttemptExist = "VALID_TWO_FACTOR_SMS_ATTEMPT_EXISTS";

        /// <summary>
        /// Indicates that a two-factor authentication attempt via email was invalid.
        /// </summary>
        public static readonly string TwoFactorEmailAttemptInvalid = "TWO_FACTOR_EMAIL_ATTEMPT_INVALID";

        /// <summary>
        /// Indicates that a two-factor authentication attempt via SMS was invalid.
        /// </summary>
        public static readonly string TwoFactorSmsAttemptInvalid = "TWO_FACTOR_SMS_ATTEMPT_INVALID";

        /// <summary>
        /// Indicates that a valid email confirmation attempt already exists, preventing a new request.
        /// </summary>
        public static readonly string ValidEmailConfirmationAttemptExists = "VALID_EMAIL_CONFIRMATION_EXISTS";

        /// <summary>
        /// Indicates that the provided backup code does not exist or is invalid.
        /// </summary>
        public static readonly string BackupCodeDoesNotExist = "BACKUP_CODE_DOES_NOT_EXIST";

        /// <summary>
        /// When a users phone number is already confirmed
        /// </summary>
        public static readonly string PhoneNumberConfirmed = "PHONE_NUMBER_CONFIRMED";

        /// <summary>
        /// A a phone confirmation attempt dose not exist 
        /// </summary>
        public static readonly string ConfirmPhoneNumberAttemptDoseNotExist = "PHONE_NUMBER_CONFIRMATION_DOSE_NOT_EXIST";
    }
}
