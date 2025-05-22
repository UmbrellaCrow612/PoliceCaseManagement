import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { UserService } from '../../../../../../core/user/services/user.service';
import { RestrictedUser, User } from '../../../../../../core/user/type';
import { MatListModule, MatSelectionListChange } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';

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
  constructor(private readonly userService: UserService) {}

  /**
   * Caller will pass you a list of currently assigned user id's for the case so you can hide them in the UI
   */
  readonly currentAssignedUserIds = inject<{
    currentAssignedUserIds: string[];
  }>(MAT_DIALOG_DATA).currentAssignedUserIds;

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
            this.searchedUsers = users.filter(
              (user) => !this.currentAssignedUserIds.includes(user.id) // we remove those already on this case from parent
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

    selectedMatListUsers.forEach((user) => {
      if (!this.selectedUsersMap.has(user.id)) {
        this.selectedUsersMap.set(user.id, user);
      }
    });
  }

  /**
   * Fired off when a user is selected then clicked to be removed off the selection list
   * @param user User to remove
   */
  removedSelectedUser(user: RestrictedUser) {
    this.selectedUsersMap.delete(user.id);
  }
}
