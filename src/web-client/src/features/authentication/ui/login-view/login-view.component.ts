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
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import CODES from '../../../../core/server-responses/codes';
import { appPaths } from '../../../../core/app/constants/appPaths';
import { MatSnackBar } from '@angular/material/snack-bar';

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
    private snackBar: MatSnackBar
  ) {}

  isLoggingIn = false;
  incorrectCredentialsError = false;

  loginForm = new FormGroup({
    email: new FormControl('', [Validators.email]),
    password: new FormControl('', [Validators.required]),
  });

  onSubmit() {
    if (this.loginForm.valid) {
      this.isLoggingIn = true;
      this.incorrectCredentialsError = false;

      this.authService
        .Login(
          this.loginForm.controls.email.getRawValue()!,
          this.loginForm.controls.password.getRawValue()!
        )
        .subscribe({
          next: (config) => {
            this.isLoggingIn = false;
            this.router.navigate(['../two-factor'], {
              relativeTo: this.route,
              queryParams: { loginAttemptId: config.loginAttemptId },
            });
          },
          error: (err: HttpErrorResponse) => {
            this.isLoggingIn = false;

            let code = err.error.errors[0]?.code;

            switch (code) {
              case CODES.IncorrectCreds || CODES.UserDoseNotExist:
                this.incorrectCredentialsError = true;
                break;

              case CODES.EmailNotConfirmed:
                this.router.navigate([`../${appPaths.CONFIRM_EMAIL}`], {
                  relativeTo: this.route,
                });
                break;

              case CODES.PhoneNumberNotConfirmed:
                this.router.navigate([`../${appPaths.PHONE_CONFIRMATION}`], {
                  relativeTo: this.route,
                });
                break;

              case CODES.DEVICE_NOT_CONFIRMED:
                this.router.navigate([`../${appPaths.DEVICE_CHALLENGE}`], {
                  relativeTo: this.route,
                });
                break;

              case CODES.REQUIRES_PASSWORD_CHANGE:
                this.router.navigate([`../`, appPaths.CHANGE_PASSWORD], {
                  relativeTo: this.route,
                });
                break;

              case CODES.EXPIRED_PASSWORD_BEING_USED:
                this.router.navigate([`../`, appPaths.CHANGE_PASSWORD], {
                  relativeTo: this.route,
                });
                break;

              default:
                this.snackBar.open(`Failed error: ${code}`);
                break;
            }
          },
        });
    }
  }
}
