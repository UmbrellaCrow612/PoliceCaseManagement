import { Routes } from '@angular/router';

export const ADMINISTRATION_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import(
        './components/administration-shell/administration-shell.component'
      ).then((c) => c.AdministrationShellComponent),
    title: 'Administration',
  },
];
