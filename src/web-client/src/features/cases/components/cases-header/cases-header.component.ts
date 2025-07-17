import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { AppLink } from '../../../../core/app/type';
import { MatMenuModule } from '@angular/material/menu';
import { RouterLink, RouterModule } from '@angular/router';
import { HeaderProfileComponent } from '../../../../core/components/header-profile/header-profile.component';

interface CaseMenuLink extends AppLink {
  children: CaseMenuLink[]; // one layer of sub children supported

  icon: string;
}

@Component({
  selector: 'app-cases-header',
  imports: [
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    RouterModule,
    HeaderProfileComponent,
  ],
  templateUrl: './cases-header.component.html',
  styleUrl: './cases-header.component.css',
})
export class CasesHeaderComponent {
  menuLinks: Array<CaseMenuLink> = [
    {
      authorizedRoles: [],
      href: '/cases',
      name: 'Home',
      children: [],
      icon: 'home',
    },
    {
      authorizedRoles: [],
      href: '/cases/me',
      name: 'My Cases',
      children: [],
      icon: 'person',
    },
    {
      authorizedRoles: [],
      href: '/cases/search',
      name: 'Search',
      children: [],
      icon: 'search',
    },
  ];
}
