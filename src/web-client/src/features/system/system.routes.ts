import { Routes } from '@angular/router';

export const SYSTEM_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./components/system-shell/system-shell.component').then(
        (c) => c.SystemShellComponent
      ),
    title: 'System',

    children: [
      {
        path: '',
        loadComponent: () =>
          import('./ui/system-home-view/system-home-view.component').then(
            (c) => c.SystemHomeViewComponent
          ),
      },
      {
        path: 'cases/incident-type',
        loadComponent: () =>
          import(
            './ui/system-incident-type-view/system-incident-type-view.component'
          ).then((c) => c.SystemIncidentTypeViewComponent),
      },
    ],
  },
];
