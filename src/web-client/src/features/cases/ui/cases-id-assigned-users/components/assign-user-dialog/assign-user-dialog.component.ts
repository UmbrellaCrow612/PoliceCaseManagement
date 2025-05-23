import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import {
  MAT_DIALOG_DATA,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { UserService } from '../../../../../../core/user/services/user.service';
import { RestrictedUser, User } from '../../../../../../core/user/type';
import { MatListModule, MatSelectionListChange } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { CaseService } from '../../../../../../core/cases/services/case.service';
import { formatBackendError } from '../../../../../../core/app/errors/formatError';

@Component({
  selector: 'app-assign-user-dialog',
  imports: [
    MatDialogModule,
    MatButtonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    CommonModule,
    MatListModule,
    MatIconModule,
  ],
  templateUrl: './assign-user-dialog.component.html',
  styleUrl: './assign-user-dialog.component.css',
})
export class AssignUserDialogComponent {
  constructor(
    private readonly userService: UserService,
    private readonly caseService: CaseService
  ) {}

  /**
   * Caller will pass you a list of currently assigned user id's for the case so you can hide them in the UI
   */
  readonly data = inject<{
    currentAssignedUserIds: string[];
    caseId: string;
  }>(MAT_DIALOG_DATA);

  /**
   * Ref to the current dialog
   */
  readonly dialogRef = inject(MatDialogRef<AssignUserDialogComponent>);

  /**
   * List of selected user to be assigned to this case
   */
  selectedUsersMap = new Map<string, RestrictedUser>();
  searchUsersForm = new FormGroup({
    search: new FormControl('', [Validators.required]),
  });
  isFetchingUsers = false;
  fetchingUsersError: string | null = null;
  searchedUsers: RestrictedUser[] = [];
  isSavingUsers = false;
  isSavingError: string | null = null;

  /**
   * Ran when the user submits the search form
   */
  searchUsersFormSubmit() {
    if (this.searchUsersForm.valid) {
      this.isFetchingUsers = true;
      this.fetchingUsersError = null;

      this.userService
        .searchUsersByUsername(this.searchUsersForm.controls.search.value!)
        .subscribe({
          next: (users) => {
            const assignedUserIds = new Set(this.data.currentAssignedUserIds);
            this.searchedUsers = users.filter(
              (user) => !assignedUserIds.has(user.id)
            );
            this.isFetchingUsers = false;
          },
          error: (error) => {
            error = 'Failed to search users';
            this.isFetchingUsers = false;
          },
        });
    }
  }

  /**
   *
   * @returns retuns a list of selcted users
   */
  fromMap() {
    return Array.from(this.selectedUsersMap.values());
  }

  /**
   * Fired off every time a user is selected or de seclected from the mat list
   * @param event EventEmitter<MatSelectionListChange>
   */
  selectedUsersChangedListener(event: MatSelectionListChange) {
    const selectedMatListUsers: RestrictedUser[] =
      event.source.selectedOptions.selected.map((option) => option.value);

    const selectedIds = new Set(selectedMatListUsers.map((u) => u.id));

    selectedMatListUsers.forEach((user) => {
      if (!this.selectedUsersMap.has(user.id)) {
        this.selectedUsersMap.set(user.id, user);
      }
    });

    Array.from(this.selectedUsersMap.values()).forEach((user) => {
      if (
        this.searchedUsers.find((u) => u.id === user.id) &&
        !selectedIds.has(user.id)
      ) {
        this.selectedUsersMap.delete(user.id);
      }
    });
  }

  /**
   * fired off when you want to save assignerd users to a case
   */
  saveClicked() {
    if (!this.data.caseId) {
      console.error('Case ID not passed');
      return;
    }

    if (this.selectedUsersMap.size > 0) {
      this.isSavingUsers = true;
      this.isSavingError = null;

      this.caseService
        .assignUsersToCase(this.data.caseId, this.fromMap())
        .subscribe({
          next: () => {
            this.dialogRef.close();
          },
          error: (error) => {
            (this.isSavingError = formatBackendError(error)),
              (this.isSavingUsers = false);
          },
        });
    }
  }

  /**
   * Fired off when a user is selected then clicked to be removed off the selection list
   * @param user User to remove
   */
  removedSelectedUser(user: RestrictedUser) {
    this.selectedUsersMap.delete(user.id);
  }
}
