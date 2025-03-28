import { Component, OnInit } from '@angular/core';
import { CanComponentDeactivate } from '../../../../../core/app/guards/canDeactivateGuard';
import { Observable, timer } from 'rxjs';
import { MatTabsModule } from '@angular/material/tabs';
import { MatButtonModule } from '@angular/material/button';
import { UserManagementEditUserDetailsViewComponent } from './ui/user-management-edit-user-details-view/user-management-edit-user-details-view.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { User } from '../../../../../core/user/type';
import { UserService } from '../../../../../core/user/services/user.service';
import { ActivatedRoute } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-user-management-edit-user-view',
  imports: [
    MatTabsModule,
    MatButtonModule,
    UserManagementEditUserDetailsViewComponent,
    MatProgressSpinnerModule,
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

    this.userService.getUserById({ userId: this.userId! }).subscribe({
      next: (val) => {
        this.userDetailsData = val;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'User not found';
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

  userDataChangeEventListener(isUserEditFormValid: boolean) {
    this.hasUnsavedChanges = true;

    if (!isUserEditFormValid) {
      this.isFormsValid['userDetailsFormValid'] = false;
    } else {
      this.isFormsValid['userDetailsFormValid'] = true;
    }
  }

  ngOnInit(): void {
    this.fetchData();
  }

  saveClick() {
    if (this.hasUnsavedChanges && this.areAllFormsValid(this.isFormsValid)) {
      this.snackBar.open('Saved changes', 'Close', { duration: 10000 });
      this.cancelClick();
    } else {
      this.snackBar.open(
        'No Changes made or Invalid form data please see and amend fields in red',
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
