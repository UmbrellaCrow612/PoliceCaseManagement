import { Routes } from '@angular/router';
import { appPaths } from '../../core/app/constants/appPaths';

export const AUTHENTICATION_ROUTES: Routes = [
  {
    path: appPaths.LOGIN,
    loadComponent: () =>
      import('./ui/login-view/login-view.component').then(
        (c) => c.LoginViewComponent
      ),
  },
  {
    path: appPaths.CONFIRM_EMAIL, 
    loadComponent: () =>
      import('./ui/confirm-email-view/confirm-email-view.component').then(
        (c) => c.ConfirmEmailViewComponent
      ),
  },
  {
    path: appPaths.DEVICE_CHALLENGE,
    loadComponent: () =>
      import('./ui/device-challenge-view/device-challenge-view.component').then(
        (c) => c.DeviceChallengeViewComponent
      ),
  },
  {
    path: appPaths.CONFIRM_DEVICE_CHALLENGE,
    loadComponent: () =>
      import('./ui/confirm-device-challenge-view/confirm-device-challenge-view.component').then(
        (c) => c.ConfirmDeviceChallengeViewComponent
      ),
  },
  {
    path: appPaths.PHONE_CONFIRMATION, 
    loadComponent: () =>
      import(
        './ui/phone-confirmation-view/phone-confirmation-view.component'
      ).then((c) => c.PhoneConfirmationViewComponent),
  },
  {
    path: appPaths.RESET_PASSWORD,
    loadComponent: () =>
      import('./ui/reset-password-view/reset-password-view.component').then(
        (c) => c.ResetPasswordViewComponent
      ),
  },
  {
    path: appPaths.TWO_FACTOR,
    loadComponent: () =>
      import('./ui/two-factor-view/two-factor-view.component').then(
        (c) => c.TwoFactorViewComponent
      ),
  },
  {
    path: appPaths.TWO_FACTOR_SMS(),
    loadComponent: () =>
      import('./ui/two-factor-sms-view/two-factor-sms-view.component').then(
        (c) => c.TwoFactorSmsViewComponent
      ),
  },
  {
    path: appPaths.TWO_FACTOR_EMAIL(),
    loadComponent: () =>
      import('./ui/two-factor-email-view/two-factor-email-view.component').then(
        (c) => c.TwoFactorEmailViewComponent
      ),
  },
  {
    path: appPaths.TWO_FACTOR_TOTP(),
    loadComponent: () =>
      import('./ui/two-factor-totpview/two-factor-totpview.component').then(
        (c) => c.TwoFactorTOTPViewComponent
      ),
  }
];
