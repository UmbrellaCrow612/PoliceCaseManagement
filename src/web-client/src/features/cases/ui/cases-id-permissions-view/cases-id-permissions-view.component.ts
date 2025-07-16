import { Component, inject, OnInit } from '@angular/core';
import { CaseService } from '../../../../core/cases/services/case.service';
import { ActivatedRoute } from '@angular/router';
import {
  CasePermission,
  CasePermissionNames,
} from '../../../../core/cases/type';
import { formatBackendError } from '../../../../core/app/errors/formatError';
import { CommonModule } from '@angular/common';
import { BackNavigationButtonComponent } from '../../../../core/components/back-navigation-button/back-navigation-button.component';
import { CasePermissionItemComponent } from './components/case-permission-item/case-permission-item.component';
import { UserService } from '../../../../core/user/services/user.service';
import { forkJoin } from 'rxjs';
import { AuthenticationService } from '../../../../core/authentication/services/authentication.service';
import { getBusinessErrorCode } from '../../../../core/server-responses/getBusinessErrorCode';
import CODES from '../../../../core/server-responses/codes';

@Component({
  selector: 'app-cases-id-permissions-view',
  imports: [
    CommonModule,
    BackNavigationButtonComponent,
    CasePermissionItemComponent,
  ],
  templateUrl: './cases-id-permissions-view.component.html',
  styleUrl: './cases-id-permissions-view.component.css',
})
export class CasesIdPermissionsViewComponent implements OnInit {
  constructor(
    private readonly caseService: CaseService,
    private readonly active: ActivatedRoute,
    private readonly authService: AuthenticationService
  ) {}
  ngOnInit(): void {
    this.caseId = this.active.snapshot.paramMap.get('caseId');

    this.fetchData();
  }

  /**
   * Used to filter out permission of trhe current user so they cannot change there own
   */
  currentUserId = inject(UserService).USER?.id;
  permissions: CasePermission[] = [];

  /**
   * Get the current users permissions for the given case
   */
  currentUserCasePermissions: string[] = [];

  caseId: string | null = null;

  isLoading = false;
  error: string | null = null;

  /**
   * Fired off from the child when the perms have up[dated so we refresh stale data
   */
  permissionUpdatedListner() {
    this.fetchData();
  }

  fetchData() {
    if (!this.caseId) {
      this.error = 'Case ID not extracted from URL';
      return;
    }

    this.isLoading = true;
    this.error = null;

    forkJoin([
      this.caseService.getPermissions(this.caseId),
      this.caseService.getCurrentUsersRoleForCase(this.caseId),
    ]).subscribe({
      next: ([perms, currentUserPerms]) => {
        this.permissions = perms.filter((x) => x.userId !== this.currentUserId); // remove current user perms from UI
        this.currentUserCasePermissions = currentUserPerms;
        this.isLoading = false;
      },
      error: (err) => {
        let code = getBusinessErrorCode(err);

        switch (code) {
          case CODES.CASE_PERMISSION:
            this.authService.UnAuthorized();
            break;

          default:
            this.error = formatBackendError(err);
            this.isLoading = false;
            break;
        }
      },
    });
  }
}
