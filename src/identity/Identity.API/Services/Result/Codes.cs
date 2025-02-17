namespace Identity.API.Services.Result
{
    /// <summary>
    /// Defines standard business rule exception codes used across the project in <see cref="IServiceError.Code"/>.
    /// These codes represent various failure scenarios, including authentication, verification, and validation errors.
    /// </summary>
    public static class Codes
    {
        /// <summary>
        /// Authentication-related error codes.
        /// </summary>
        public static class Authentication
        {
            /// <summary>
            /// Indicates that the provided credentials are incorrect.
            /// </summary>
            public const string IncorrectCredentials = "INCORRECT_CREDENTIALS";

            /// <summary>
            /// Indicates that the account has been locked due to security policies or excessive failed attempts.
            /// </summary>
            public const string AccountLocked = "ACCOUNT_LOCKED";

            /// <summary>
            /// Indicates that the attempted login operation is not valid.
            /// </summary>
            public const string LoginAttemptNotValid = "LOGIN_ATTEMPT_NOT_VALID";
        }

        /// <summary>
        /// Verification-related error codes.
        /// </summary>
        public static class Verification
        {
            /// <summary>
            /// Indicates that the user's email address has not been confirmed.
            /// </summary>
            public const string EmailNotConfirmed = "EMAIL_NOT_CONFIRMED";

            /// <summary>
            /// Indicates that the user's phone number has not been confirmed.
            /// </summary>
            public const string PhoneNumberNotConfirmed = "PHONE_NOT_CONFIRMED";

            /// <summary>
            /// Indicates that the device used for an operation has not been confirmed.
            /// </summary>
            public const string DeviceNotConfirmed = "DEVICE_NOT_CONFIRMED";

            /// <summary>
            /// Represents an email confirmation process or token-related issue.
            /// </summary>
            public const string EmailConfirmation = "EMAIL_CONFIRMATION";

            /// <summary>
            /// Indicates that the email address has already been confirmed and does not require further verification.
            /// </summary>
            public const string EmailAlreadyConfirmed = "EMAIL_ALREADY_CONFIRMED";

            /// <summary>
            /// Indicates that the user's phone number has already been confirmed.
            /// </summary>
            public const string PhoneNumberConfirmed = "PHONE_NUMBER_CONFIRMED";

            /// <summary>
            /// Indicates that a phone number confirmation attempt does not exist.
            /// </summary>
            public const string ConfirmPhoneNumberAttemptDoesNotExist = "PHONE_NUMBER_CONFIRMATION_DOES_NOT_EXIST";

            /// <summary>
            /// Indicates that valid phone verifaction attempt still exists.
            /// </summary>
            public const string ValidConfirmationPhoneNumberAttemptExists = "VALID_PHONE_CONFIRMATION_EXISTS";

            /// <summary>
            /// Indicates that device is already trusted.
            /// </summary>
            public const string DeviceAlreadyTrusted = "DEVICE_ALREADY_TRUSTED";

            /// <summary>
            /// Indicates that a verifaction attempt for this device already exists.
            /// </summary>
            public const string ValidDeviceConfirmationAttemptExists= "VALID_DEVICE_CONFIRMATION_EXISTS";

            /// <summary>
            /// Indicates that a verifaction attempt for this device dose not exist.
            /// </summary>
            public const string DeviceConfirmationAttemptDoseNotExists = "DEVICE_CONFIRMATION_DOSE_NOT_EXIST";
        }

        /// <summary>
        /// Two-factor authentication-related error codes.
        /// </summary>
        public static class TwoFactor
        {
            /// <summary>
            /// Indicates that a valid two-factor authentication attempt via email confirmation already exists.
            /// </summary>
            public const string ValidTwoFactorEmailAttemptExists = "VALID_TWO_FACTOR_EMAIL_CONFIRMATION_ALREADY_EXISTS";

            /// <summary>
            /// Indicates that a valid two-factor authentication attempt via SMS already exists.
            /// </summary>
            public const string ValidTwoFactorSmsAttemptExists = "VALID_TWO_FACTOR_SMS_ATTEMPT_EXISTS";

            /// <summary>
            /// Indicates that a two-factor authentication attempt via email was invalid.
            /// </summary>
            public const string TwoFactorEmailAttemptInvalid = "TWO_FACTOR_EMAIL_ATTEMPT_INVALID";

            /// <summary>
            /// Indicates that a two-factor authentication attempt via SMS was invalid.
            /// </summary>
            public const string TwoFactorSmsAttemptInvalid = "TWO_FACTOR_SMS_ATTEMPT_INVALID";

            /// <summary>
            /// Indicates that a valid email confirmation attempt already exists, preventing a new request.
            /// </summary>
            public const string ValidEmailConfirmationAttemptExists = "VALID_EMAIL_CONFIRMATION_EXISTS";

            /// <summary>
            /// Indicates that the provided backup code does not exist or is invalid.
            /// </summary>
            public const string BackupCodeDoesNotExist = "BACKUP_CODE_DOES_NOT_EXIST";
        }

        /// <summary>
        /// Validation-related error codes.
        /// </summary>
        public static class Validation
        {
            /// <summary>
            /// Indicates that a validation error occurred, typically due to invalid input data.
            /// </summary>
            public const string ValidationError = "VALIDATION_ERROR";

            /// <summary>
            /// Indicates that the specified user does not exist.
            /// </summary>
            public const string UserDoesNotExist = "USER_DOES_NOT_EXIST";
        }
    }
}
