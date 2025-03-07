import { Component } from '@angular/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { AuthenticationService } from '../../../../core/authentication/services/authentication.service';
import { LoginCredentials } from '../../../../core/authentication/types';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ActivatedRoute, Router } from '@angular/router';
import formatBackendError from '../../../../core/server-responses/errors/utils/format-error';
import { HttpErrorResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { ErrorDialogComponent } from '../../../../core/components/error-dialog/error-dialog.component';
import CODES from '../../../../core/server-responses/codes';
import { appPaths } from '../../../../core/app/constants/appPaths';

@Component({
  selector: 'app-login-view',
  imports: [
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './login-view.component.html',
  styleUrl: './login-view.component.css',
})
export class LoginViewComponent {
  constructor(
    private authService: AuthenticationService,
    private router: Router,
    private route: ActivatedRoute,
    private dialog: MatDialog
  ) {}

  isLoggingIn = false;
  incorrectCredentialsError = false;

  loginCredentials: LoginCredentials = {
    email: '',
    password: '',
  };

  loginForm = new FormGroup({
    email: new FormControl('', [Validators.email]),
    password: new FormControl('', [Validators.required]),
  });

  onSubmit() {
    if (this.loginForm.valid) {
      this.loginCredentials.email = this.loginForm.value.email!;
      this.loginCredentials.password = this.loginForm.value.password!;

      this.isLoggingIn = true;
      this.incorrectCredentialsError = false;

      this.authService.Login(this.loginCredentials).subscribe({
        next: (config) => {
          this.isLoggingIn = false;
          this.router.navigate(['../two-factor'], {
            relativeTo: this.route,
            queryParams: { loginAttemptId: config.id },
          });
        },
        error: (err: HttpErrorResponse) => {
          this.isLoggingIn = false;

          if (
            err.error[0]?.code == CODES.IncorrectCreds || // we expect these codes
            err.error[0]?.code == CODES.UserDoseNotExist
          ) {
            this.incorrectCredentialsError = true;
          } else if (err.error[0]?.code == CODES.EmailNotConfirmed) {
            this.router.navigate([`../${appPaths.CONFIRM_EMAIL}`], {
              relativeTo: this.route,
            });
          } else {
            let errorMessage = formatBackendError(err);
            this.dialog.open(ErrorDialogComponent, {
              data: errorMessage,
            });
          }
        },
      });
    }
  }
}
