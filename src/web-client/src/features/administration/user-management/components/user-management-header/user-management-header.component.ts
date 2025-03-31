import { Component } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { AppLink } from '../../../../../core/app/type';
import { RouterModule } from '@angular/router';
import { appPaths } from '../../../../../core/app/constants/appPaths';
import { Location } from '@angular/common';

@Component({
  selector: 'app-user-management-header',
  imports: [MatToolbarModule, MatButtonModule, MatIconModule, RouterModule],
  templateUrl: './user-management-header.component.html',
  styleUrl: './user-management-header.component.css',
})
export class UserManagementHeaderComponent {
  constructor(private location: Location) {}
  links: Array<AppLink> = [
    {
      href: `/${appPaths.ADMINISTRATION}/${appPaths.A_USER_MANAGEMENT}`,
      authorizedRoles: [],
      name: 'Home',
    },
    {
      href: 'create',
      authorizedRoles: [],
      name: 'Add new user',
    },
  ];

  goBack() {
    this.location.back();
  }
}
