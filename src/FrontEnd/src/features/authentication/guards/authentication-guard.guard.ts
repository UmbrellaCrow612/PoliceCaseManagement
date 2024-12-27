import { CanActivateFn } from '@angular/router';

export const authenticationGuardGuard: CanActivateFn = (route, state) => {
  return true;
};
