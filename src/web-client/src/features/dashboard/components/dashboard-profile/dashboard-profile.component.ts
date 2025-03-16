import { AppLink } from './../../../../core/app/type';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { UserService } from '../../../../core/user/services/user.service';
import { hasRequiredRole } from '../../../../core/authentication/utils';
import { UserRoles } from '../../../../core/authentication/roles';
import { RouterModule } from '@angular/router';

interface DashboardProfileLink extends AppLink {
  /**
   * Icon to render with the link
   */
  iconName: string;
}

@Component({
  selector: 'app-dashboard-profile',
  imports: [MatButtonModule, MatIconModule, MatMenuModule, RouterModule],
  templateUrl: './dashboard-profile.component.html',
  styleUrl: './dashboard-profile.component.css',
})
export class DashboardProfileComponent {
  constructor() {}
  ROLES: string[] = inject(UserService).ROLES!;
  hasRequiredRole = hasRequiredRole;

  links: Array<DashboardProfileLink> = [
    {
      name: 'Administration',
      href: '/administration',
      authorizedRoles: [UserRoles.Admin],
      iconName: 'shield_person', // from material icons
    },
  ];
}
