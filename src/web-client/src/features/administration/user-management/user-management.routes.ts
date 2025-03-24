import { Routes } from '@angular/router';

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
      },
    ],
  },
];
