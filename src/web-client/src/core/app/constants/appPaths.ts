/**
 * Our app paths
 **/
export const appPaths = {
  AUTHENTICATION: 'authentication',
  LOGIN: 'login',
  CONFIRM_EMAIL: 'confirm-email',
  DEVICE_CHALLENGE: 'device-challenge',
  CONFIRM_DEVICE_CHALLENGE: 'confirm-device-challenge',
  PHONE_CONFIRMATION: 'phone-confirmation',
  RESET_PASSWORD: 'reset-password',
  TWO_FACTOR: 'two-factor',
  TWO_FACTOR_SMS() {
    return `${this.TWO_FACTOR}/sms`;
  },
  TWO_FACTOR_EMAIL() {
    return `${this.TWO_FACTOR}/email`;
  },
  TWO_FACTOR_TOTP(){
    return `${this.TWO_FACTOR}/totp`;
  }
} as const;
