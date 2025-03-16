import { Routes } from '@angular/router';

export const DASHBOARD_ROUTES: Routes = [
  {
    path: '',
    title: 'Dashboard', // default / for /dashboard to be rendered
    loadComponent: () =>
      import(
        './components/dashboard-skeleton/dashboard-skeleton.component'
      ).then((c) => c.DashboardSkeletonComponent),
    children: [
      {
        path: '', // default fist page to render in /dashboard
        loadComponent: () =>
          import(
            './ui/dashboard-overview-view/dashboard-overview-view.component'
          ).then((c) => c.DashboardOverviewViewComponent),
      },
    ],
  },
];
