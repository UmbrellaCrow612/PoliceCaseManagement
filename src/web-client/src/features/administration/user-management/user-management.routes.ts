import { Routes } from '@angular/router';
import { UserManagementEditUserViewComponent } from './ui/user-management-edit-user-view/user-management-edit-user-view.component';
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
        path: 'users/:userId/edit',
        loadComponent: () =>
          import(
            './ui/user-management-edit-user-view/user-management-edit-user-view.component'
          ).then((c) => c.UserManagementEditUserViewComponent),
        canDeactivate: [canDeactivateGuard],
      },
    ],
  },
];
