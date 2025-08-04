import { Routes } from '@angular/router';

export const PEOPLE_ROUTES: Routes = [
  {
    title: 'People management',
    path: '',
    loadComponent: () =>
      import('./components/people-shell/people-shell.component').then(
        (c) => c.PeopleShellComponent
      ),
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
