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
    },
    {
      name: 'Administration',
      href: '/administration',
      authorizedRoles: [UserRoles.Admin],
      iconName: 'shield_person',
    },
    {
      name: 'Dashboard',
      href: '/dashboard',
      authorizedRoles: [],
      iconName: 'dashboard',
    },
    {
      name: 'Cases',
      href: '/cases',
      authorizedRoles: [],
      iconName: 'cases',
    },
    {
      name: 'System',
      href: '/system',
      authorizedRoles: [UserRoles.Admin],
      iconName: 'hub',
    },
    {
      name: 'Evidence',
      href: '/evidence',
      authorizedRoles: [],
      iconName: 'description',
    },
    {
      name: 'People',
      href: '/people',
      authorizedRoles: [],
      iconName: 'group',
    },
  ];

  logout() {
    this.authService.Logout();
  }
}
