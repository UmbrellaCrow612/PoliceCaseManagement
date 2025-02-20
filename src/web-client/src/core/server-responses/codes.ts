// Mapped from backend
const CODES = {
  DeviceNotConfirmed: 'DEVICE_NOT_CONFIRMED',
  PhoneNumberNotConfirmed: "PHONE_NOT_CONFIRMED",
  EmailNotConfirmed: "EMAIL_NOT_CONFIRMED",
  AccountLocked: "ACCOUNT_LOCKED",
  IncorrectCreds: "INCORRECT_CREDENTIALS",
  UserDoseNotExist: "USER_DOES_NOT_EXIST"
} as const;

export default CODES;
