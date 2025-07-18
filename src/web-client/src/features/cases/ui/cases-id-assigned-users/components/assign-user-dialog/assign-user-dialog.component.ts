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
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { CaseService } from '../../../../../../core/cases/services/case.service';
import { RestrictedUser } from '../../../../../../core/user/type';

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
   * Holds loading state for fetching data
   */
  isLoading = false;

  /**
   * Holds any error state
   */
  errorMessage: string | null = null;

  /**
   * Form to be used to get the search form results
   */
  searchUsersForm = new FormGroup({
    search: new FormControl('', [Validators.required]),
  });

  /**
   * Control to hold the current selected user
   */
  selectedUserControl = new FormControl<RestrictedUser | null>(null, [
    Validators.required,
  ]);

  /**
   * List of users fetched for the search term
   */
  searchUsersList: RestrictedUser[] = [];

  /**
   * Holds state for saving user state
   */
  isSaving = false;

  /**
   * Ran when the user submits the search form
   */
  searchUsersFormSubmit() {
    if (this.searchUsersForm.valid) {
      this.isLoading = true;
      this.errorMessage = null;

      this.userService
        .searchUsersByUsername(this.searchUsersForm.controls.search.value!)
        .subscribe({
          next: (users) => {
            let alreadyAssignedUserIdsSet = new Set(
              this.data.currentAssignedUserIds
            );

            this.searchUsersList = users.filter(
              // filter out users already assigned to the case
              (x) => !alreadyAssignedUserIdsSet.has(x.id)
            );

            this.isLoading = false;
          },
          error: (error) => {
            this.errorMessage = 'Failed to fetch users from backend';
            this.isLoading = false;
          },
        });
    }
  }

  /**
   * fired off when you want to save assignerd users to a case
   */
  saveClicked() {
    if (!this.data.caseId || this.selectedUserControl.invalid) {
      console.error('Invalid state');
      return;
    }

    this.isSaving = true;
    this.errorMessage = null;

    // Extract the first user from the array (since multiple=false, there should only be one)
    // multi select ends up retuning a array
    const selectedUser = Array.isArray(this.selectedUserControl.value)
      ? this.selectedUserControl.value[0]
      : this.selectedUserControl.value;

    this.caseService
      .assignUserToCase(this.data.caseId, selectedUser)
      .subscribe({
        next: () => {
          this.dialogRef.close();
        },
        error: () => {
          this.errorMessage = 'Failed to assign user to case';
          this.isSaving = false;
        },
      });
  }
}
