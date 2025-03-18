import { AuthenticationService } from './../services/authentication.service';
import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { UserService } from '../../user/services/user.service';
import { hasRequiredRole } from '../utils';

/**
 * This will make sure:
 * - A user is authenticated at least before accessing a route
 * - They have at least one roles required for said route
 * 
 * NOTE:
 * - Make sure the route itself has data: { requiredRoles: ["Roles"] } passed to it
 * 
 * @param route
 * @returns {Boolean} A bool if a user can or cannot access a specified route
 *
 * @example
 {
     path: 'admin',
     loadComponent: () =>
       import('./ui/login-view/login-view.component').then(
         (c) => c.LoginViewComponent
       ),
     canActivate: [rolesAuthorizationGuard],
     data: {
       requiredRoles: [UserRoles.Admin],
     },
   },
 */
export const rolesAuthorizationGuard: CanActivateFn = (
  route,
  state
): boolean => {
  const requiredRoles = route.data['requiredRoles'];
  if (!requiredRoles) {
    console.error('Roles not passed to rolesAuthorizationGuard guard');
    return false;
  }

  let userService = inject(UserService);
  if (userService.USER === null || userService.ROLES === null) {
    console.log('Current user not authenticated cannot access this route');
    inject(AuthenticationService).Logout();
    return false;
  }

  if (!hasRequiredRole(requiredRoles, userService.ROLES)) {
    console.log('Current user dose not have required roles for this view');

    // TODO go to UnAuthorized page using auth service function
    return false;
  }

  return true;
};
