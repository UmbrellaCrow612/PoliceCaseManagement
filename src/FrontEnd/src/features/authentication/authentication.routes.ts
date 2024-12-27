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
    path: 'device-challenge', // have query params in URL for device confirmation
    loadComponent: () =>
      import('./ui/device-challenge-view/device-challenge-view.component').then(
        (c) => c.DeviceChallengeViewComponent
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
];
