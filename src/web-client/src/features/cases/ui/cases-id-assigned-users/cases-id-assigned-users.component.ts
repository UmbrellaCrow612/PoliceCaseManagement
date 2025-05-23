import { Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { AssignUserDialogComponent } from './components/assign-user-dialog/assign-user-dialog.component';
import { ActivatedRoute } from '@angular/router';
import { CaseService } from '../../../../core/cases/services/case.service';
import { CaseUser, RestrictedUser } from '../../../../core/user/type';
import { CommonModule } from '@angular/common';
import { formatBackendError } from '../../../../core/app/errors/formatError';

@Component({
  selector: 'app-cases-id-assigned-users',
  imports: [MatButtonModule, CommonModule],
  templateUrl: './cases-id-assigned-users.component.html',
  styleUrl: './cases-id-assigned-users.component.css',
})
export class CasesIdAssignedUsersComponent implements OnInit {
  constructor(
    private readonly dialog: MatDialog,
    private active: ActivatedRoute,
    private readonly CaseService: CaseService
  ) {}
  ngOnInit(): void {
    this.caseId = this.active.snapshot.paramMap.get('caseId');

    this.fetchData();
  }

  caseId: string | null = null;
  assignedUsers: CaseUser[] = [];
  isLoading = true;
  error: string | null = null;
  /**
   * Fetches page data
   */
  fetchData() {
    if (!this.caseId) {
      this.error = 'Case ID not present';
      return;
    }

    this.isLoading = true;
    this.error = null;

    this.CaseService.getAssignedUsers(this.caseId).subscribe({
      next: (users) => {
        this.assignedUsers = users;
        this.isLoading = false;
      },
      error: (err) => {
        this.error = formatBackendError(err);
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
}
