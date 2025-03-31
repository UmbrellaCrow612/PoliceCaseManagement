import { Component } from '@angular/core';
import { MatListModule } from '@angular/material/list';
import { User } from '../../../../../core/user/type';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { UserService } from '../../../../../core/user/services/user.service';
import { HttpErrorResponse } from '@angular/common/http';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-user-management-home-view',
  imports: [
    MatListModule,
    MatProgressSpinnerModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    RouterModule
  ],
  templateUrl: './user-management-home-view.component.html',
  styleUrl: './user-management-home-view.component.css',
})
export class UserManagementHomeViewComponent {
  constructor(private userService: UserService) {}

  isLoading = false;
  errorMessage: string | null = null;
  users: User[] = [];

  searchUsersForm = new FormGroup({
    userName: new FormControl<string | null>(null),
    email: new FormControl<string | null>(null),
    phoneNumber: new FormControl<string | null>(null),
  });

  onSubmit() {
    this.fetchData();
  }

  fetchData() {
    this.isLoading = true;
    this.errorMessage = null;
    this.users = [];

    this.userService
      .searchUsersByQuery({
        email: this.searchUsersForm.controls.email.getRawValue(),
        phoneNumber: this.searchUsersForm.controls.phoneNumber.getRawValue(),
        userName: this.searchUsersForm.controls.userName.getRawValue(),
      })
      .subscribe({
        next: (users) => {
          this.users = users;
          this.isLoading = false;
        },
        error: (err: HttpErrorResponse) => {
          this.errorMessage = `Failed to get users code: ${err.error[0]?.code}`;
          this.isLoading = false;
        },
      });
  }
}
