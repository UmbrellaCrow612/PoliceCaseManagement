import { Routes } from '@angular/router';
import { HomeViewComponent } from './home-view/home-view.component';

export const HomeRoutes: Routes = [
  {
    path: '',
    component: HomeViewComponent,
    title: 'Configy',
  },
];
