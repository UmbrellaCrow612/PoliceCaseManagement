import { Component, inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { AssignUserDialogComponent } from './components/assign-user-dialog/assign-user-dialog.component';
import { ActivatedRoute } from '@angular/router';
import { CaseService } from '../../../../core/cases/services/case.service';
import { CaseAcessList, RestrictedUser } from '../../../../core/user/type';
import { CommonModule } from '@angular/common';
import { formatBackendError } from '../../../../core/app/errors/formatError';
import { BackNavigationButtonComponent } from '../../../../core/components/back-navigation-button/back-navigation-button.component';
import { RemoveAssignedUserDialogComponent } from './components/remove-assigned-user-dialog/remove-assigned-user-dialog.component';
import { UserService } from '../../../../core/user/services/user.service';
import { CaseRolePipe } from '../../../../core/cases/pipes/caseRolePipe';
import { forkJoin } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { StatusCodes } from '../../../../core/http/codes/status-codes';
import { AuthenticationService } from '../../../../core/authentication/services/authentication.service';
import { CaseRoleNames, CaseRoleValue } from '../../../../core/cases/type';

@Component({
  selector: 'app-cases-id-assigned-users',
  imports: [
    MatButtonModule,
    CommonModule,
    BackNavigationButtonComponent,
    CaseRolePipe,
  ],
  templateUrl: './cases-id-assigned-users.component.html',
  styleUrl: './cases-id-assigned-users.component.css',
})
export class CasesIdAssignedUsersComponent implements OnInit {
  constructor(
    private readonly dialog: MatDialog,
    private active: ActivatedRoute,
    private readonly CaseService: CaseService,
    private readonly authService: AuthenticationService
  ) {}
  ngOnInit(): void {
    this.caseId = this.active.snapshot.paramMap.get('caseId');

    this.fetchData();
  }

  /**
   * The current case from the URL dynamic params
   */
  caseId: string | null = null;

  /**
   * List of users who are linked to the given case
   */
  assignedUsers: CaseAcessList[] = [];

  /**
   * Holds any loading state
   */
  isLoading = true;

  /**
   * Holds any error state
   */
  errorMessage: string | null = null;

  /**
   * Current users role on the given case
   */
  currentUserCaseRole: CaseRoleValue | null = null;

  /**
   * Current context loged in user
   */
  currentUserId = inject(UserService).USER?.id;

  /**
   * Local copy of CaseRoleNames to be used in the UI
   */
  copy_CaseRoleNames = CaseRoleNames;

  /**
   * Fetches page data
   */
  fetchData() {
    if (!this.caseId) {
      this.errorMessage = 'Case ID not present';
      return;
    }

    this.isLoading = true;
    this.errorMessage = null;

    forkJoin({
      users: this.CaseService.getAssignedUsers(this.caseId),
      currentUserRoleOnCase: this.CaseService.getCurrentUsersRoleForCase(
        this.caseId
      ),
    }).subscribe({
      next: (response) => {
        (this.assignedUsers = response.users),
          (this.currentUserCaseRole = response.currentUserRoleOnCase.role);

        this.isLoading = false;
      },
      error: (error: HttpErrorResponse) => {
        if (error.status == StatusCodes.FORBIDDEN) {
          this.authService.UnAuthorized();
          return;
        }

        this.errorMessage = formatBackendError(error);
        this.isLoading = false;
      },
    });
  }

  assignUserClicked() {
    this.dialog
      .open(AssignUserDialogComponent, {
        width: '100%',
        maxWidth: '500px',

        data: {
          currentAssignedUserIds: this.assignedUsers.map((x) => x.userId),
          caseId: this.caseId,
        },
      })
      .afterClosed()
      .subscribe(() => {
        this.fetchData();
      });
  }

  /**
   * Runs when the remove button is clicked
   * @param userId The ID of the user to remove
   */
  removeUserClicked(userId: string) {
    this.dialog
      .open(RemoveAssignedUserDialogComponent, {
        width: '100%',
        maxWidth: '500px',
        data: {
          userId: userId,
          caseId: this.caseId,
        },
      })
      .afterClosed()
      .subscribe(() => {
        this.fetchData();
      });
  }
}
