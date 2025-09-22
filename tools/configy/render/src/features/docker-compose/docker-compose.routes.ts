import { Routes } from '@angular/router';
import { DockerComposeViewComponent } from './docker-compose-view/docker-compose-view.component';

export const DockerComposeRoutes: Routes = [
  {
    path: '',
    component: DockerComposeViewComponent,
    title: 'Docker compose',
  },
];
