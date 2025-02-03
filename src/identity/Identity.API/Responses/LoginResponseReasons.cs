namespace Identity.API.Responses
{
    public static class LoginResponseReasons
    {
        public static readonly string IncorrectCreds = "INCORRECT_CREDENTIALS";
        public static readonly string AccountLocked = "ACCOUNT_LOCKED";
        public static readonly string EmailNotConfirmed = "EMAIL_NOT_CONFIRMED";
        public static readonly string PhoneNumberNotConfirmed = "PHONE_NOT_CONFIRMED";
        public static readonly string DeviceNotConfirmed = "DEVICE_NOT_CONFIRMED";
        public static readonly string EmailConfirmation = "EMAIL_CONFIRMATION";
    }
}
