import { Routes } from '@angular/router';

export const PROFILE_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./components/profile-shell/profile-shell.component').then(
        (c) => c.ProfileShellComponent
      ),
    children: [
      {
        path: '',
        loadComponent: () =>
          import('./ui/profile-home-page/profile-home-page.component').then(
            (c) => c.ProfileHomePageComponent
          ),
      },
    ],
  },
];
