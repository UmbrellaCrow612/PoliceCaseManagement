import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadChildren: () =>
      import('../features/home/home.routes').then((m) => m.HomeRoutes),
  },
  {
    path: 'launch-settings',
    loadChildren: () =>
      import('../features/launch-settings/launch-settings.routes').then(
        (m) => m.LaunchSettingsRoutes
      ),
  },
];
