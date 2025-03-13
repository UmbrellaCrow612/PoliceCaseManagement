import { Component, OnInit } from '@angular/core';
import {
  FormControl,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AuthenticationService } from '../../../../core/authentication/services/authentication.service';
import {
  SendPhoneConfirmationRequest,
  ValidatePhoneConfirmationCodeRequestBody,
} from '../../../../core/authentication/types';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import CODES from '../../../../core/server-responses/codes';
import { appPaths } from '../../../../core/app/constants/appPaths';

@Component({
  selector: 'app-phone-confirmation-view',
  imports: [
    MatCardModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
  ],
  templateUrl: './phone-confirmation-view.component.html',
  styleUrl: './phone-confirmation-view.component.css',
})
export class PhoneConfirmationViewComponent {
  constructor(
    private authService: AuthenticationService,
    private router: Router,
    private active: ActivatedRoute
  ) {}
  phoneNumberInput = new FormControl(null, [Validators.required]);
  codeInput = new FormControl(null, [Validators.required]);
  isSendPhoneConfirmationRequest = false;
  isSendingValidatePhoneConfirmationCodeRequest = false;
  showEnterPhoneConfirmationCodeInput: boolean = false;
  errorMessage: null | string = null;

  changePhoneNumber() {
    this.showEnterPhoneConfirmationCodeInput = false;
  }

  sendPhoneConfirmation() {
    if (this.phoneNumberInput.valid) {
      this.isSendPhoneConfirmationRequest = true;
      this.errorMessage = null;

      let body: SendPhoneConfirmationRequest = {
        phoneNumber: `${this.phoneNumberInput.value!}`,
      };
      this.authService.SendPhoneConfirmation(body).subscribe({
        next: () => {
          this.isSendPhoneConfirmationRequest = false;
          this.showEnterPhoneConfirmationCodeInput = true;
        },
        error: (err: HttpErrorResponse) => {
          this.isSendPhoneConfirmationRequest = false;

          let code = err.error[0]?.code;

          switch (code) {
            case CODES.PHONE_NUMBER_ALREADY_CONFIRMED:
              this.router.navigate([`../${appPaths.LOGIN}`], {
                relativeTo: this.active,
              });
              break;

            case CODES.VALID_PHONE_CONFIRMATION_EXISTS:
              this.errorMessage =
                'Valid phone confirmation exists wait for 2 minutes';
              this.showEnterPhoneConfirmationCodeInput = true;
              break;

            /** For security we don't reval user dose not exist */
            case CODES.USER_DOES_NOT_EXIST:
              this.errorMessage = null;
              this.showEnterPhoneConfirmationCodeInput = true;
              break;

            default:
              this.errorMessage = `Code: ${code} unknown error`;
              break;
          }
        },
      });
    }
  }

  validateCode() {
    if (this.codeInput.valid) {
      this.isSendingValidatePhoneConfirmationCodeRequest = true;
      this.errorMessage = null;

      let body: ValidatePhoneConfirmationCodeRequestBody = {
        phoneNumber: `${this.phoneNumberInput.getRawValue()}`,
        code: this.codeInput.getRawValue()!,
      };
      this.authService.ValidatePhoneConfirmationCode(body).subscribe({
        next: () => {
          this.router.navigate([`../${appPaths.LOGIN}`], {
            relativeTo: this.active,
          });
        },
        error: (err: HttpErrorResponse) => {
          let code = err.error[0].code;

          switch (code) {
            case CODES.PHONE_NUMBER_ALREADY_CONFIRMED:
              this.router.navigate([`../${appPaths.LOGIN}`], {
                relativeTo: this.active,
              });
              break;

            case CODES.PHONE_NUMBER_CONFIRMATION_DOES_NOT_EXIST:
              this.errorMessage = 'Invalid Code provided';
              break;

            case CODES.USER_DOES_NOT_EXIST:
              this.errorMessage = 'Invalid Code provided';
              break;

            default:
              this.errorMessage = `Unhandled error: ${JSON.stringify(
                err.error
              )}`;
              break;
          }
        },
      });
    }
  }
}
