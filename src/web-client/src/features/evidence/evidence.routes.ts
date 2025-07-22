import { Routes } from '@angular/router';

export const EVIDENCE_ROUTES: Routes = [
  {
    path: '',
    title: 'evidence',
    loadComponent: () =>
      import('./components/evidence-shell/evidence-shell.component').then(
        (e) => e.EvidenceShellComponent
      ),
    children: [
      {
        path: '',
        loadComponent: () =>
          import('./ui/evidence-home-view/evidence-home-view.component').then(
            (e) => e.EvidenceHomeViewComponent
          ),
      },
      {
        path: ':evidenceId',
        loadComponent: () =>
          import('./ui/evidence-id-view/evidence-id-view.component').then(
            (e) => e.EvidenceIdViewComponent
          ),
      },
    ],
  },
];
