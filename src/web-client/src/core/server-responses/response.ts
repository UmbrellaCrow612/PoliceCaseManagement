// Mapped from backend
const REASONS = {
  DeviceNotConfirmed: 'DEVICE_NOT_CONFIRMED',
  PhoneNumberNotConfirmed: "PHONE_NOT_CONFIRMED",
  EmailNotConfirmed: "EMAIL_NOT_CONFIRMED",
  AccountLocked: "ACCOUNT_LOCKED",
  IncorrectCreds: "INCORRECT_CREDENTIALS"
} as const;

export default REASONS;
