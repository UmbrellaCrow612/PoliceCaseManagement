import { Routes } from '@angular/router';

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
        path: ':caseId',
        loadComponent: () =>
          import('./ui/cases-id-view/cases-id-view.component').then(
            (c) => c.CasesIdViewComponent
          ),
      },
    ],
  },
];
