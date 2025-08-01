import { Routes } from '@angular/router';

export const PEOPLE_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./components/people-shell/people-shell.component').then(
        (c) => c.PeopleShellComponent
      ),
    title: 'People management',
    children: [
      {
        path: '',
        loadComponent: () =>
          import('./ui/people-home-view/people-home-view.component').then(
            (c) => c.PeopleHomeViewComponent
          ),
      },
    ],
  },
];
