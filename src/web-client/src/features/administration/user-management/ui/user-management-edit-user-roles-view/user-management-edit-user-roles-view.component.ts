import { Component, OnInit } from '@angular/core';
import { UserService } from '../../../../../core/user/services/user.service';
import { ActivatedRoute } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { UserRoles } from '../../../../../core/authentication/roles';
import {
  MatCheckboxChange,
  MatCheckboxModule,
} from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';
import { CanComponentDeactivate } from '../../../../../core/app/guards/canDeactivateGuard';
import { forkJoin, Observable } from 'rxjs';
import { User } from '../../../../../core/user/type';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-user-management-edit-user-roles-view',
  imports: [MatButtonModule, MatCheckboxModule],
  templateUrl: './user-management-edit-user-roles-view.component.html',
  styleUrl: './user-management-edit-user-roles-view.component.css',
})
export class UserManagementEditUserRolesViewComponent
  implements OnInit, CanComponentDeactivate
{
  constructor(
    private userService: UserService,
    private active: ActivatedRoute,
    private snackBar: MatSnackBar
  ) {}

  isLoading = true;
  errorMessage: string | null = null;
  userId: string | null = null;
  systemRoles = UserRoles.all();
  selectedRoles = new Set<string>();
  hasUnsavedChanges = false;
  user: User | null = null;

  ngOnInit(): void {
    this.userId = this.active.snapshot.paramMap.get('userId');
    if (!this.userId) {
      this.errorMessage = 'Failed to get userId from URL';
      this.isLoading = false;
      return;
    }
    this.fetchData();
  }

  onRoleChange(role: string, event: MatCheckboxChange) {
    if (event.checked) {
      this.selectedRoles.add(role);
    } else {
      this.selectedRoles.delete(role);
    }
    this.hasUnsavedChanges = true;
  }

  fetchData() {
    if (!this.userId) return;

    this.isLoading = true;
    this.errorMessage = null;
    this.hasUnsavedChanges = false;
    this.selectedRoles.clear();

    forkJoin({
      user: this.userService.getUserById({ userId: this.userId! }),
      rolesResponse: this.userService.getUserRolesById({
        userId: this.userId!,
      }),
    }).subscribe({
      next: (response) => {
        (this.user = response.user),
          (this.selectedRoles = new Set(response.rolesResponse.roles)),
          (this.isLoading = false);
      },
      error: (err: HttpErrorResponse) => {
        this.errorMessage = `Failed to fetch user and roles code: ${err.error[0]?.code}`;
        this.isLoading = false;
      },
    });
  }

  saveClicked() {
    this.isLoading = true;

    this.userService
      .updateUserRoles(this.user!, Array.from(this.selectedRoles))
      .subscribe({
        next: () => {
          this.snackBar.open('Updated user roles', 'Close', {
            duration: 10000,
          });
          this.fetchData();
        },
        error: (err: HttpErrorResponse) => {
          this.errorMessage = `Failed to update user code: ${err.error[0]?.code}`;
          this.isLoading = false;
        },
      });
  }

  canDeactivate(): boolean | Observable<boolean> {
    if (this.hasUnsavedChanges) {
      return confirm(
        'Are you sure you want to leave you have unsaved changes.'
      );
    }

    return true;
  }
}
