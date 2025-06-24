import { Component, inject } from '@angular/core';
import { hasRequiredRole } from '../../authentication/utils/hasRequiredRole';
import { UserService } from '../../user/services/user.service';
import { AppLink } from '../../app/type';
import { UserRoles } from '../../authentication/roles';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { RouterModule } from '@angular/router';
import { AuthenticationService } from '../../authentication/services/authentication.service';

interface ProfileLink extends AppLink {
  /**
   * Icon to render with the link
   */
  iconName: string;
}

@Component({
  selector: 'app-header-profile',
  imports: [MatButtonModule, MatIconModule, MatMenuModule, RouterModule],
  templateUrl: './header-profile.component.html',
  styleUrl: './header-profile.component.css',
})
export class HeaderProfileComponent {
  constructor(private authService: AuthenticationService) {}

  ROLES: string[] = inject(UserService).ROLES!;
  hasRequiredRole = hasRequiredRole;

  links: Array<ProfileLink> = [
    {
      name: 'Profile',
      href: '/profile',
      authorizedRoles: [],
      iconName: 'person',
      permissionsNeeded: []
    },
    {
      name: 'Administration',
      href: '/administration',
      authorizedRoles: [UserRoles.Admin],
      iconName: 'shield_person',
      permissionsNeeded: []
    },
    {
      name: 'Dashboard',
      href: '/dashboard',
      authorizedRoles: [],
      iconName: 'dashboard',
      permissionsNeeded: []
    },
    {
      name: 'Cases',
      href: '/cases',
      authorizedRoles: [],
      iconName: 'cases',
      permissionsNeeded: []
    },
    {
      name: 'System',
      href: '/system',
      authorizedRoles: [UserRoles.Admin],
      iconName: 'hub',
      permissionsNeeded: []
    },
    {
      name: 'Evidence',
      href: '/evidence',
      authorizedRoles: [],
      iconName: 'description',
      permissionsNeeded: []
    },
  ];

  logout() {
    this.authService.Logout();
  }
}
