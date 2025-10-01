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
  {
    path: 'config-drift',
    loadChildren: () =>
      import('../features/config-drift/config-drift.routes').then(
        (m) => m.ConfigDriftRoutes
      ),
  },
];
