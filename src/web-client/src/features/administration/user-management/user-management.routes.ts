import { Routes } from '@angular/router';
import { canDeactivateGuard } from '../../../core/app/guards/canDeactivateGuard';

export const USER_MANAGEMENT_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import(
        './components/user-management-shell/user-management-shell.component'
      ).then((c) => c.UserManagementShellComponent),

    children: [
      {
        path: '',
        loadComponent: () =>
          import(
            './ui/user-management-home-view/user-management-home-view.component'
          ).then((c) => c.UserManagementHomeViewComponent),
      },
      {
        path: 'create',
        loadComponent: () =>
          import(
            './ui/user-management-create-view/user-management-create-view.component'
          ).then((c) => c.UserManagementCreateViewComponent),
        canDeactivate: [canDeactivateGuard],
      },
      {
        path: 'users/:userId',
        loadComponent: () =>
          import(
            './ui/user-management-user-id-view/user-management-user-id-view.component'
          ).then((c) => c.UserManagementUserIdViewComponent),
      },
      {
        path: 'users/:userId/edit/details',
        loadComponent: () =>
          import(
            './ui/user-management-edit-user-details-view/user-management-edit-user-details-view.component'
          ).then((c) => c.UserManagementEditUserDetailsViewComponent),
        canDeactivate: [canDeactivateGuard],
      },
      {
        path: 'users/:userId/edit/roles',
        loadComponent: () =>
          import(
            './ui/user-management-edit-user-roles-view/user-management-edit-user-roles-view.component'
          ).then((c) => c.UserManagementEditUserRolesViewComponent),
      },
    ],
  },
];
