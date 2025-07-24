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
        path: 'me',
        loadComponent: () =>
          import('./ui/cases-me-view/cases-me-view.component').then(
            (c) => c.CasesMeViewComponent
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
        path: ':caseId/actions',
        loadComponent: () =>
          import(
            './ui/cases-id-actions-view/cases-id-actions-view.component'
          ).then((c) => c.CasesIdActionsViewComponent),
      },
      {
        path: ':caseId/assigned-users',
        loadComponent: () =>
          import(
            './ui/cases-id-assigned-users/cases-id-assigned-users.component'
          ).then((c) => c.CasesIdAssignedUsersComponent),
      },
      {
        path: ':caseId/attachments',
        loadComponent: () =>
          import(
            './ui/cases-id-attachments-view/cases-id-attachments-view.component'
          ).then((c) => c.CasesIdAttachmentsViewComponent),
      },
      {
        path: ':caseId/evidence',
        loadComponent: () =>
          import(
            './ui/cases-id-evidence-view/cases-id-evidence-view.component'
          ).then((c) => c.CasesIdEvidenceViewComponent),
      },
    ],
  },
];
