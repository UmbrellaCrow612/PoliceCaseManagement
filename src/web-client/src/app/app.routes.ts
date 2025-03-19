import { Routes } from '@angular/router';
import { appPaths } from '../core/app/constants/appPaths';
import { rolesAuthorizationGuard } from '../core/authentication/guards/roles-authorization.guard';
import { UserRoles } from '../core/authentication/roles';

export const routes: Routes = [
  {
    path: appPaths.AUTHENTICATION,
    loadChildren: () =>
      import('../features/authentication/authentication.routes').then(
        (m) => m.AUTHENTICATION_ROUTES
      ),
  },
  {
    path: appPaths.DASHBOARD,
    loadChildren: () =>
      import('../features/dashboard/dashboard.routes').then(
        (r) => r.DASHBOARD_ROUTES
      ),
    canActivate: [rolesAuthorizationGuard],
    data: {
      requiredRoles: [], // Means they have to be at least authenticated
    },
  },
  {
    path: appPaths.ADMINISTRATION,
    loadChildren: () =>
      import('../features/administration/administration.routes').then(
        (r) => r.ADMINISTRATION_ROUTES
      ),
    canActivate: [rolesAuthorizationGuard],
    data: {
      requiredRoles: [UserRoles.Admin], // Means there authenticated and have admin role
    },
  },
];
