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

      /**
       * Sub feature of system - all system incident type routes
       */
      {
        path: 'cases/incident-types',
        loadChildren: () =>
          import('./incident-type/system-incident-type.routes').then(
            (m) => m.SYSTEM_INCIDENT_TYPE_ROUTES
          ),
      },
    ],
  },
];
