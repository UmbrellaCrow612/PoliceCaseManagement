import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { appPaths } from '../../../../core/app/constants/appPaths';
import { RouterLink } from '@angular/router';
import { AppLink } from '../../../../core/app/type';
import { UserService } from '../../../../core/user/services/user.service';
import { hasRequiredRole } from '../../../../core/authentication/utils/hasRequiredRole';
import { UserRoles } from '../../../../core/authentication/roles';
import { fromEvent, Subscription } from 'rxjs';
import { HeaderProfileComponent } from '../../../../core/components/header-profile/header-profile.component';

@Component({
  selector: 'app-dashboard-header',
  imports: [MatIconModule, RouterLink, HeaderProfileComponent],
  templateUrl: './dashboard-header.component.html',
  styleUrl: './dashboard-header.component.css',
})
export class DashboardHeaderComponent implements OnInit, OnDestroy {
  dashboardUrl = `/${appPaths.DASHBOARD}`;
  ROLES: string[] = inject(UserService).ROLES!;
  windowWidth: number = window.innerWidth; // width of window an used for rendering for responsive design
  private resizeSubscription: Subscription | null = null;

  ngOnInit() {
    this.resizeSubscription = fromEvent(window, 'resize').subscribe(() => {
      this.windowWidth = window.innerWidth;
    });
  }

  /**
   * We need to assign it locally because we cannot use it from diff file directly in template
   */
  hasRequiredRole = hasRequiredRole;

  links: Array<AppLink> = [
    {
      name: 'Cases',
      authorizedRoles: [],
      href: `/${appPaths.DASHBOARD_CASES()}`,
    },
    {
      name: 'Officers',
      authorizedRoles: [], // TODO: here in future we could restrict to managers above officers as personal details or a role that is above a officer
      href: `/${appPaths.DASHBOARD_OFFICER()}`,
    },
    {
      name: 'Evidence',
      authorizedRoles: [], // TODO: here in future we could restrict to managers above officers as personal details or a role that is above a officer
      href: `/${appPaths.DASHBOARD_EVIDENCE()}`,
    },
    {
      name: 'Crime',
      authorizedRoles: [], // TODO: here in future we could restrict to managers above officers as personal details or a role that is above a officer
      href: `/${appPaths.DASHBOARD_CRIME()}`,
    },
    {
      name: 'Suspect & Witness',
      authorizedRoles: [], // TODO: here in future we could restrict to managers above officers as personal details or a role that is above a officer
      href: `/${appPaths.DASHBOARD_SUSPECT_AND_WITNESS()}`,
    },
    {
      name: 'Victim',
      authorizedRoles: [], // TODO: here in future we could restrict to managers above officers as personal details or a role that is above a officer
      href: `/${appPaths.DASHBOARD_VICTIM()}`,
    },
    {
      name: 'Task & Assignment',
      authorizedRoles: [], // TODO: here in future we could restrict to managers above officers as personal details or a role that is above a officer
      href: `/${appPaths.DASHBOARD_TASK_AND_ASSIGNMENT()}`,
    },
    {
      name: 'Legal & Court',
      authorizedRoles: [], // TODO: here in future we could restrict to managers above officers as personal details or a role that is above a officer
      href: `/${appPaths.DASHBOARD_LEGAL_AND_COURT()}`,
    },
    {
      name: 'Administration & User',
      authorizedRoles: [UserRoles.Admin],
      href: `/${appPaths.DASHBOARD_ADMIN()}`,
    },
  ];

  ngOnDestroy(): void {
    if (this.resizeSubscription) {
      this.resizeSubscription.unsubscribe();
    }
  }
}
