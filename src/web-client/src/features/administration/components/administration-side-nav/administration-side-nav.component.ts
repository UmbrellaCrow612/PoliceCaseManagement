import { Component } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { AppLink } from '../../../../core/app/type';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { appPaths } from '../../../../core/app/constants/appPaths';

interface SideNavLink extends AppLink {
  iconName: string;
}

@Component({
  selector: 'app-administration-side-nav',
  imports: [MatIconModule, MatButtonModule, RouterModule],
  templateUrl: './administration-side-nav.component.html',
  styleUrl: './administration-side-nav.component.css',
})
export class AdministrationSideNavComponent {
  links: Array<SideNavLink> = [
    {
      href: appPaths.A_USER_MANAGEMENT,
      authorizedRoles: [],
      name: 'User management',
      iconName: 'group',
    },
  ];
}
