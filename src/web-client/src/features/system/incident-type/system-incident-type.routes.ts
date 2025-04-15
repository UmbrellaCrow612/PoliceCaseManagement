import { Routes } from '@angular/router';

export const SYSTEM_INCIDENT_TYPE_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import(
        './ui/system-incident-type-home-view/system-incident-type-home-view.component'
      ).then((c) => c.SystemIncidentTypeHomeViewComponent),
    title: 'System incident types',
  },
];
