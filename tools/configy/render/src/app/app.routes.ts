import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadChildren: () =>
      import('../featuers/start/start.routes').then((m) => m.START_ROUTES),
  },
];
