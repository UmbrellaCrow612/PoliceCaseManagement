import { SendEmailConfirmationRequest } from './../../../../core/authentication/types';
import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AuthenticationService } from '../../../../core/authentication/services/authentication.service';
import { HttpErrorResponse, HttpResponse } from '@angular/common/http';
import CODES from '../../../../core/server-responses/codes';
import { ErrorDialogComponent } from '../../../../core/components/error-dialog/error-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-confirm-email-view',
  imports: [
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  templateUrl: './confirm-email-view.component.html',
  styleUrl: './confirm-email-view.component.css',
})
export class ConfirmEmailViewComponent {
  constructor(
    private authService: AuthenticationService,
    private dialog: MatDialog
  ) {}
  isSendingRequest = false;
  errorMessage: string | null = '';

  emailConfirmation = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
  });

  onSubmit() {
    if (this.emailConfirmation.valid) {
      this.isSendingRequest = true;
      this.errorMessage = null;

      let body: SendEmailConfirmationRequest = {
        email: this.emailConfirmation.controls.email.getRawValue()!,
      };

      this.authService.SendConfirmationEmail(body).subscribe({
        next: () => {
          this.isSendingRequest = false;
        },
        error: (error: HttpErrorResponse) => {
          this.isSendingRequest = false;

          let code = error.error[0]?.code;

          if (code === CODES.VALID_EMAIL_CONFIRMATION_EXISTS) {
            this.errorMessage = 'Valid email attempt sent wait for 2 minutes';
            return;
          }

          let errMessage = `Status: ${error.status} codes: ${JSON.stringify(
            error.error
          )}`;

          this.dialog.open(ErrorDialogComponent, {
            data: errMessage,
          });
        },
      });
    }
  }
}
