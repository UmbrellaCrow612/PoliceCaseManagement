import { Component } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Validator_password } from '../../../../core/app/validators/controls';
import { MatIconModule } from '@angular/material/icon';
import { AuthenticationService } from '../../../../core/authentication/services/authentication.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { timer } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-change-password-view',
  imports: [
    ReactiveFormsModule,
    MatInputModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
  ],
  templateUrl: './change-password-view.component.html',
  styleUrl: './change-password-view.component.css',
})
export class ChangePasswordViewComponent {
  constructor(
    private authService: AuthenticationService,
    private active: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}
  changePasswordForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required]),
    newPassword: new FormControl('', [Validators.required, Validator_password]),
  });

  newPasswordInputType: string = 'password';
  isSendingRequest = false;
  errorMessage: string | null = null;

  toggleInputType() {
    if (this.newPasswordInputType === 'password') {
      this.newPasswordInputType = 'text';
    } else {
      this.newPasswordInputType = 'password';
    }
  }

  onSubmit() {
    this.isSendingRequest = true;
    this.errorMessage = null;

    if (this.changePasswordForm.valid) {
      this.authService.ChangePassword(this.changePasswordForm.value).subscribe({
        next: () => {
          this.isSendingRequest = false;
          this.snackBar.open('Changed password', 'Close', {
            duration: 5000,
          });
          timer(2500).subscribe(() => {
            this.router.navigate(['../', 'login'], {
              relativeTo: this.active,
            });
          });
        },
        error: (err: HttpErrorResponse) => {
          this.errorMessage = `Error code: ${err.error[0]?.code}`;
          this.isSendingRequest = false;
        },
      });
    } else {
      this.isSendingRequest = false;
      this.errorMessage = 'Form invalid';
    }
  }
}
