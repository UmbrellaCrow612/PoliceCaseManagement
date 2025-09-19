import { Routes } from '@angular/router';
import { StartViewComponent } from './views/start-view/start-view.component';

export const START_ROUTES: Routes = [
  {
    path: '',
    component: StartViewComponent,
    title: 'Configy welcome',
  },
];
