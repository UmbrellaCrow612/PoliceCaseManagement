/**
 * Our app paths
 **/
export const appPaths = {
  AUTHENTICATION: 'authentication',
  LOGIN: 'login',
  CONFIRM_EMAIL: 'confirm-email',
  CONFIRM_EMAIL_WITH_CODE: 'confirm-email-code',
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
  TWO_FACTOR_TOTP() {
    return `${this.TWO_FACTOR}/totp`;
  },

  DASHBOARD: 'dashboard',
  DASHBOARD_CASES() {
    return `${this.DASHBOARD}/cases`;
  },
  DASHBOARD_OFFICER() {
    return `${this.DASHBOARD}/officers`;
  },
  DASHBOARD_EVIDENCE() {
    return `${this.DASHBOARD}/evidence`;
  },
  DASHBOARD_CRIME() {
    return `${this.DASHBOARD}/crime`;
  },
  DASHBOARD_SUSPECT_AND_WITNESS() {
    return `${this.DASHBOARD}/suspect-witness`;
  },
  DASHBOARD_VICTIM() {
    return `${this.DASHBOARD}/victim`;
  },
  DASHBOARD_TASK_AND_ASSIGNMENT() {
    return `${this.DASHBOARD}/task-assignment`;
  },
  DASHBOARD_LEGAL_AND_COURT() {
    return `${this.DASHBOARD}/legal-court`;
  },
  DASHBOARD_ADMIN() {
    return `${this.DASHBOARD}/admin`;
  },

  UNAUTHORIZED: "unauthorized",

  ADMINISTRATION: 'administration',
} as const;
