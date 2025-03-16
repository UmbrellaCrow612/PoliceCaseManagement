import { MatFormFieldModule } from '@angular/material/form-field';
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import {
  FormControl,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from '../../../../core/authentication/services/authentication.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { appPaths } from '../../../../core/app/constants/appPaths';
import {
  SendTwoFactorSmsCodeRequestBody,
  ValidateTwoFactorSmsCodeRequestBody,
} from '../../../../core/authentication/types';
import { HttpErrorResponse } from '@angular/common/http';
import CODES from '../../../../core/server-responses/codes';
import { interval, Subscription, timer } from 'rxjs';

@Component({
  selector: 'app-two-factor-sms-view',
  imports: [
    MatCardModule,
    MatButtonModule,
    ReactiveFormsModule,
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
  ],
  templateUrl: './two-factor-sms-view.component.html',
  styleUrl: './two-factor-sms-view.component.css',
})
export class TwoFactorSmsViewComponent implements OnInit {
  constructor(
    private router: Router,
    private active: ActivatedRoute,
    private authService: AuthenticationService,
    private snackBar: MatSnackBar
  ) {}

  loginAttemptId: string | null = null;
  codeInput = new FormControl(null, {
    validators: [Validators.required],
  });
  disableConfirmButton = false;
  disableResendCodeButton = false;
  countDown = 60;
  countDownSub: null | Subscription = null;

  ngOnInit(): void {
    this.active.queryParamMap.subscribe((val) => {
      this.loginAttemptId = val.get('loginAttemptId');

      if (!this.loginAttemptId) {
        this.router.navigate([`../../${appPaths.LOGIN}`], {
          relativeTo: this.active,
        });
      }
    });
    this.sendCode();
  }

  sendCode() {
    if (this.loginAttemptId) {
      this.countDown = 60;

      this.disableResendCodeButton = true;

      this.countDownSub = interval(1000).subscribe(() => {
        this.countDown--;
      });

      timer(60000).subscribe(() => {
        this.disableResendCodeButton = false;
        if (this.countDownSub) {
          this.countDownSub.unsubscribe();
        }
      });

      let body: SendTwoFactorSmsCodeRequestBody = {
        loginAttemptId: this.loginAttemptId,
      };

      this.authService.SendTwoFactorSmsCode(body).subscribe({
        next: () => {
          this.snackBar.open('Sms code sent', 'Close', {
            duration: 4500,
          });
        },
        error: (err: HttpErrorResponse) => {
          let code = err.error[0]?.code;

          switch (code) {
            case CODES.LOGIN_ATTEMPT_NOT_VALID:
              this.router.navigate([`../../${appPaths.LOGIN}`], {
                relativeTo: this.active,
              });
              break;

            case CODES.USER_DOES_NOT_EXIST:
              this.router.navigate([`../../${appPaths.LOGIN}`], {
                relativeTo: this.active,
              });
              break;

            case CODES.PHONE_NOT_CONFIRMED:
              this.router.navigate([`../../${appPaths.PHONE_CONFIRMATION}`], {
                relativeTo: this.active,
              });
              break;

            case CODES.VALID_TWO_FACTOR_SMS_ATTEMPT_EXISTS:
              this.snackBar.open(
                'Valid two factor code sent, wait for 2 minutes to send a new one',
                'Close',
                {
                  duration: 5000,
                }
              );
              break;

            default:
              this.snackBar.open(`Unhandled error ${err.error}`, 'Close', {
                duration: 10000,
              });
              break;
          }
        },
      });
    }
  }

  validateCode() {
    if (this.codeInput.valid) {
      this.disableConfirmButton = true;

      let body: ValidateTwoFactorSmsCodeRequestBody = {
        loginAttemptId: this.loginAttemptId!,
        code: this.codeInput.getRawValue()!,
      };
      this.authService.ValidateTwoFactorSmsCode(body).subscribe({
        next: () => {
          this.router.navigate([`/${appPaths.DASHBOARD}`]);
        },
        error: (err: HttpErrorResponse) => {
          this.disableConfirmButton = false;

          let code = err.error[0]?.code;

          switch (code) {
            case CODES.LOGIN_ATTEMPT_NOT_VALID:
              this.router.navigate([`../../../${appPaths.LOGIN}`]);
              break;

            case CODES.USER_DOES_NOT_EXIST:
              this.router.navigate([`../../../${appPaths.LOGIN}`]);
              break;

            case CODES.TWO_FACTOR_SMS_ATTEMPT_INVALID:
              this.snackBar.open('Incorrect code', 'close', {
                duration: 5500,
              });
              break;

            default:
              this.snackBar.open(`Unhandled error: ${err.error}`);
              break;
          }
        },
      });
    }
  }
}
