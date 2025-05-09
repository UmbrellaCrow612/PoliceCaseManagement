import { Routes } from '@angular/router';
import { canDeactivateGuard } from '../../core/app/guards/canDeactivateGuard';

export const CASES_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./components/cases-shell/cases-shell.component').then(
        (c) => c.CasesShellComponent
      ),

    children: [
      {
        path: '',
        loadComponent: () =>
          import('./ui/cases-home-view/cases-home-view.component').then(
            (c) => c.CasesHomeViewComponent
          ),
      },
      {
        path: 'create',
        loadComponent: () =>
          import('./ui/cases-create-view/cases-create-view.component').then(
            (c) => c.CasesCreateViewComponent
          ),
      },
      {
        path: 'search',
        loadComponent: () =>
          import('./ui/search-cases-view/search-cases-view.component').then(
            (c) => c.SearchCasesViewComponent
          ),
      },
      {
        path: ':caseId',
        loadComponent: () =>
          import('./ui/cases-id-view/cases-id-view.component').then(
            (c) => c.CasesIdViewComponent
          ),
      },
      {
        path: ':caseId/incident-types/edit',
        loadComponent: () =>
          import(
            './ui/cases-id-edit-incident-type-view/cases-id-edit-incident-type-view.component'
          ).then((c) => c.CasesIdEditIncidentTypeViewComponent),
        canDeactivate: [canDeactivateGuard],
      },
      {
        path: ':caseId/actions',
        loadComponent: () =>
          import(
            './ui/cases-id-actions-view/cases-id-actions-view.component'
          ).then((c) => c.CasesIdActionsViewComponent),
      },
    ],
  },
];
