import { Component, OnInit } from '@angular/core';
import { CanComponentDeactivate } from '../../../../../core/app/guards/canDeactivateGuard';
import { forkJoin, Observable, timer } from 'rxjs';
import { MatTabsModule } from '@angular/material/tabs';
import { MatButtonModule } from '@angular/material/button';
import { UserManagementEditUserDetailsViewComponent } from './ui/user-management-edit-user-details-view/user-management-edit-user-details-view.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { User } from '../../../../../core/user/type';
import { UserService } from '../../../../../core/user/services/user.service';
import { ActivatedRoute } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { UserManagementEditUserRolesViewComponent } from './ui/user-management-edit-user-roles-view/user-management-edit-user-roles-view.component';
import { HttpErrorResponse } from '@angular/common/http';
import { JsonPipe } from '@angular/common';

@Component({
  selector: 'app-user-management-edit-user-view',
  imports: [
    MatTabsModule,
    MatButtonModule,
    UserManagementEditUserDetailsViewComponent,
    MatProgressSpinnerModule,
    UserManagementEditUserRolesViewComponent,
    JsonPipe,
  ],
  templateUrl: './user-management-edit-user-view.component.html',
  styleUrl: './user-management-edit-user-view.component.css',
})
export class UserManagementEditUserViewComponent
  implements CanComponentDeactivate, OnInit
{
  constructor(
    private userService: UserService,
    private active: ActivatedRoute,
    private snackBar: MatSnackBar
  ) {}
  hasUnsavedChanges = false;
  errorMessage: string | null = null;
  isLoading = true;
  userDetailsData: User | null = null;
  userId: string | null = null;
  isFormsValid: Record<string, boolean> = {
    userDetailsFormValid: false,
  };
  userRolesData: string[] = [];

  fetchData() {
    this.isLoading = true;
    this.hasUnsavedChanges = false;
    this.errorMessage = null;
    this.userDetailsData = null;
    this.userId = null;

    this.userId = this.active.snapshot.paramMap.get('userId');
    if (this.userId === null || this.userId.length < 2) {
      this.isLoading = false;
      this.errorMessage = 'User not found';
    }

    forkJoin({
      user: this.userService.getUserById({ userId: this.userId! }),
      rolesResponse: this.userService.getUserRolesById({
        userId: this.userId!,
      }),
    }).subscribe({
      next: (val) => {
        (this.userDetailsData = val.user),
          (this.userRolesData = val.rolesResponse.roles);
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Failed to fetch data';
        this.isLoading = false;
      },
    });
  }

  canDeactivate(): boolean | Observable<boolean> {
    if (this.hasUnsavedChanges) {
      return confirm('You have unsaved changes. Do you really want to leave?');
    }
    return true;
  }

  userDataChangeEventListener(event: { emitFirst: boolean; valid: boolean }) {
    if (!event.emitFirst) {
      this.hasUnsavedChanges = true;
    }
    if (!event.valid) {
      this.isFormsValid['userDetailsFormValid'] = false;
    } else {
      this.isFormsValid['userDetailsFormValid'] = true;
    }
  }

  roleChangedListener() {
    this.hasUnsavedChanges = true;
  }

  ngOnInit(): void {
    this.fetchData();
  }

  saveClick() {
    if (!this.hasUnsavedChanges) {
      return;
    }

    if (this.hasUnsavedChanges && this.areAllFormsValid(this.isFormsValid)) {
      this.isLoading = true;

      this.userService
        .updateUserAndRoles({
          user: this.userDetailsData!,
          roles: this.userRolesData,
        })
        .subscribe({
          next: () => {
            this.fetchData();
          },
          error: (err: HttpErrorResponse) => {
            this.errorMessage = `Failed to update user and roles code: ${err.error[0]?.code}`;
            this.isLoading = false;
          },
        });

      this.cancelClick();
    } else {
      this.snackBar.open(
        'Invalid form data please see and amend fields in red',
        'Close',
        { duration: 10000 }
      );
    }
  }

  cancelClick() {
    this.fetchData();
  }

  private areAllFormsValid(forms: Record<string, boolean>): boolean {
    return Object.values(forms).every((value) => value === true);
  }
}
