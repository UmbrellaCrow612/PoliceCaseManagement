import { Routes } from '@angular/router';

export const AUTHENTICATION_ROUTES: Routes = [
  {
    path: 'login',
    loadComponent: () =>
      import('./ui/login-view/login-view.component').then(
        (c) => c.LoginViewComponent
      ),
  },
  {
    path: 'confirm-email', // have query params in URL for email confirmation
    loadComponent: () =>
      import('./ui/confirm-email-view/confirm-email-view.component').then(
        (c) => c.ConfirmEmailViewComponent
      ),
  },
  {
    path: 'device-challenge',
    loadComponent: () =>
      import('./ui/device-challenge-view/device-challenge-view.component').then(
        (c) => c.DeviceChallengeViewComponent
      ),
  },
  {
    path: 'confirm-device-challenge',
    loadComponent: () =>
      import('./ui/confirm-device-challenge-view/confirm-device-challenge-view.component').then(
        (c) => c.ConfirmDeviceChallengeViewComponent
      ),
  },
  {
    path: 'phone-confirmation', // have query params in URL for phone confirmation
    loadComponent: () =>
      import(
        './ui/phone-confirmation-view/phone-confirmation-view.component'
      ).then((c) => c.PhoneConfirmationViewComponent),
  },
  {
    path: 'reset-password', // have query params in URL for password reset
    loadComponent: () =>
      import('./ui/reset-password-view/reset-password-view.component').then(
        (c) => c.ResetPasswordViewComponent
      ),
  },
  {
    path: 'two-factor',
    loadComponent: () =>
      import('./ui/two-factor-view/two-factor-view.component').then(
        (c) => c.TwoFactorViewComponent
      ),
  },
  {
    path: 'two-factor/sms',
    loadComponent: () =>
      import('./ui/two-factor-sms-view/two-factor-sms-view.component').then(
        (c) => c.TwoFactorSmsViewComponent
      ),
  },
  {
    path: 'two-factor/email',
    loadComponent: () =>
      import('./ui/two-factor-email-view/two-factor-email-view.component').then(
        (c) => c.TwoFactorEmailViewComponent
      ),
  },
  {
    path: 'two-factor/totp',
    loadComponent: () =>
      import('./ui/two-factor-totpview/two-factor-totpview.component').then(
        (c) => c.TwoFactorTOTPViewComponent
      ),
  }
];
