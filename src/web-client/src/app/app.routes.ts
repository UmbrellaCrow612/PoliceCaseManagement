import { Routes } from '@angular/router';
import { appPaths } from '../core/app/constants/appPaths';
import { rolesAuthorizationGuard } from '../core/authentication/guards/roles-authorization.guard';

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
];
