import { appPaths } from './../../core/app/constants/appPaths';
import { Routes } from '@angular/router';

export const ADMINISTRATION_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import(
        './components/administration-shell/administration-shell.component'
      ).then((c) => c.AdministrationShellComponent),
    title: 'Administration',

    children: [
      {
        path: '',
        loadComponent: () =>
          import(
            './ui/administration-home-view/administration-home-view.component'
          ).then((c) => c.AdministrationHomeViewComponent),
      },

      {
        path: appPaths.A_USER_MANAGEMENT,
        loadChildren: () =>
          import('./user-management/user-management.routes').then(
            (r) => r.USER_MANAGEMENT_ROUTES
          ),
      },
    ],
  },
];
