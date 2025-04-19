import { Routes } from '@angular/router';
import { canDeactivateGuard } from '../../../core/app/guards/canDeactivateGuard';

export const SYSTEM_INCIDENT_TYPE_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import(
        './ui/system-incident-type-home-view/system-incident-type-home-view.component'
      ).then((c) => c.SystemIncidentTypeHomeViewComponent),
    title: 'System incident types',
  },
  {
    path: 'create',
    loadComponent: () =>
      import(
        './ui/system-incident-type-create-view/system-incident-type-create-view.component'
      ).then((c) => c.SystemIncidentTypeCreateViewComponent),
    title: 'System create a incident type',
  },
  {
    path: ':incidentTypeId',
    loadComponent: () =>
      import(
        './ui/system-incident-type-id-view/system-incident-type-id-view.component'
      ).then((c) => c.SystemIncidentTypeIdViewComponent),
  },
  {
    path: ':incidentTypeId/edit',
    loadComponent: () =>
      import(
        './ui/system-incident-type-id-edit-view/system-incident-type-id-edit-view.component'
      ).then((c) => c.SystemIncidentTypeIdEditViewComponent),
    canDeactivate: [canDeactivateGuard],
  },
];
