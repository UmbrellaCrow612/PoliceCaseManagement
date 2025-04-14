import { Component } from '@angular/core';
import { AppLink } from '../../../../core/app/type';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { RouterModule } from '@angular/router';
import { HeaderProfileComponent } from '../../../../core/components/header-profile/header-profile.component';
import { appPaths } from '../../../../core/app/constants/appPaths';

interface SystemLink extends AppLink {
  children: Array<SystemLink>;

  icon: string;
}

@Component({
  selector: 'app-system-header',
  imports: [
    MatIconModule,
    MatButtonModule,
    MatMenuModule,
    RouterModule,
    HeaderProfileComponent,
  ],
  templateUrl: './system-header.component.html',
  styleUrl: './system-header.component.css',
})
export class SystemHeaderComponent {
  links: Array<SystemLink> = [
    {
      href: `/${appPaths.SYSTEM}`,
      icon: 'home',
      name: 'Home',
      authorizedRoles: [],
      children: [],
    },
    {
      href: '',
      icon: 'folder',
      name: 'Cases',
      authorizedRoles: [],
      children: [
        {
          authorizedRoles: [],
          children: [],
          href: 'cases/incident-type',
          icon: 'description',
          name: 'Incident Types',
        },
      ],
    },
  ];
}
