import { Component, OnDestroy, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { UserService } from '../../../../../core/user/services/user.service';
import { ActivatedRoute } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { CanComponentDeactivate } from '../../../../../core/app/guards/canDeactivateGuard';
import { debounceTime, Observable, Subscription, take } from 'rxjs';
import { User } from '../../../../../core/user/type';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-user-management-edit-user-details-view',
  imports: [
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    ReactiveFormsModule,
  ],
  templateUrl: './user-management-edit-user-details-view.component.html',
  styleUrl: './user-management-edit-user-details-view.component.css',
})
export class UserManagementEditUserDetailsViewComponent
  implements OnInit, CanComponentDeactivate
{
  constructor(
    private userService: UserService,
    private active: ActivatedRoute,
    private snackBar: MatSnackBar
  ) {}

  userForm = new FormGroup({
    userName: new FormControl('', [Validators.required]),
    email: new FormControl('', [Validators.required, Validators.email]),
    phoneNumber: new FormControl('', [Validators.required]),
  });
  userId: string | null = null;
  errorMessage: string | null = null;
  isLoading = true;

  ngOnInit(): void {
    this.fetchData();
  }

  fetchData() {
    this.isLoading = true;
    this.errorMessage = null;
    this.userForm.reset();

    this.userId = this.active.snapshot.paramMap.get('userId');
    if (!this.userId) {
      this.errorMessage = 'Failed to get userId from URL';
      this.isLoading = false;
    }

    if (!this.errorMessage) {
      this.userForm.disable();

      this.userService.getUserById({ userId: this.userId! }).subscribe({
        next: (user) => {
          this.userForm.patchValue(user);
          this.userForm.enable();
          this.isLoading = false;
        },
        error: (err: HttpErrorResponse) => {
          this.errorMessage = `Failed to load user code: ${err.error[0]?.code}`;
          this.isLoading = false;
        },
      });
    }
  }

  saveClicked() {
    if (this.userForm.valid) {
      this.isLoading = true;

      this.userService
        .updateUserDetails({
          id: this.userId!,
          ...this.userForm.value,
        } as User)
        .subscribe({
          next: () => {
            this.fetchData();
            this.snackBar.open('Successfully changed user details !', 'Close', {
              duration: 10000,
            });
          },
          error: (err: HttpErrorResponse) => {
            this.errorMessage = `Failed to save user code: ${err.error[0]?.code}`;
            this.isLoading = false;
          },
        });
    }
  }

  canDeactivate(): boolean | Observable<boolean> {
    if (this.userForm.dirty) {
      return confirm(
        'Are you sure you want to leave you have unsaved changes.'
      );
    }

    return true;
  }
}
