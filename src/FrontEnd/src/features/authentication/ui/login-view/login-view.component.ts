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
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';

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
    private snackBar: MatSnackBar,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  isLoggingIn = false;

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

      this.authService.Login(this.loginCredentials).subscribe({
        next: (config) => {
          this.isLoggingIn = false;
          this.router.navigate(['../two-factor'], {
            relativeTo: this.route,
            queryParams: { loginAttemptId: config.id, email: this.loginCredentials.email },
          });
        },
        error: (err) => {
          this.isLoggingIn = false;
          const errorMessage = err?.error || 'Login failed. Please try again.';
          this.snackBar.open(
            `Status code: ${err.status} Error: ${errorMessage}`,
            'Close',
            {
              duration: 5000,
              horizontalPosition: 'left',
            }
          );
        },
      });
    }
  }
}
