import { Component, OnInit } from '@angular/core';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { interval, Subscription, timer } from 'rxjs';
import { DeviceService } from '../../../../core/user/device/services/device.service';
import { ActivatedRoute, Router } from '@angular/router';
import { appPaths } from '../../../../core/app/constants/appPaths';
import { HttpErrorResponse } from '@angular/common/http';
import CODES from '../../../../core/server-responses/codes';
import { MatSnackBar } from '@angular/material/snack-bar';
import { isEmail } from '../../../../core/app/validators/isEmail';
import { getBusinessErrorCode } from '../../../../core/server-responses/getBusinessErrorCode';

@Component({
  selector: 'app-confirm-device-challenge-view',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
  ],
  templateUrl: './confirm-device-challenge-view.component.html',
  styleUrl: './confirm-device-challenge-view.component.css',
})
export class ConfirmDeviceChallengeViewComponent implements OnInit {
  constructor(
    private deviceService: DeviceService,
    private active: ActivatedRoute,
    private router: Router,
    private snackbar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.active.queryParamMap.subscribe({
      next: (value) => {
        this.email = value.get('email');
      },
    });
  }

  codeInput = new FormControl(null, [Validators.required]);
  isResendCodeButtonDisabled = false;
  countDownTimerSub: null | Subscription = null;
  countDown = 60;
  errorMessage: null | string = '';
  email: string | null = null;

  confirmCode() {
    this.errorMessage = null;
    if (this.codeInput.valid && this.email && isEmail(this.email)) {
      this.deviceService
        .ValidateDeviceChallengeCode(this.email, this.codeInput.getRawValue()!)
        .subscribe({
          next: () => {
            timer(2000).subscribe(() => {
              this.router.navigate([`../${appPaths.LOGIN}`], {
                relativeTo: this.active,
              });
            });
            this.snackbar.open('Device confirmed', 'Close', {
              duration: 1500,
            });
          },
          error: (err: HttpErrorResponse) => {
            let code = getBusinessErrorCode(err);

            switch (code) {
              case CODES.DEVICE_CONFIRMATION_DOES_NOT_EXIST:
                this.errorMessage = 'Incorrect code';
                break;

              default:
                this.errorMessage = `Unhandled error code: ${code} err: ${JSON.stringify(
                  err
                )}`;
                break;
            }
          },
        });
    } else {
      this.codeInput.setErrors({
        required: true,
      });
    }
  }

  resendCode() {
    this.isResendCodeButtonDisabled = true;
    this.countDown = 60;
    this.errorMessage = null;

    if (this.email && isEmail(this.email)) {
      this.countDownTimerSub = interval(1000).subscribe(() => {
        this.countDown--;
      });
      timer(59000).subscribe(() => {
        this.isResendCodeButtonDisabled = false;
        if (this.countDownTimerSub) {
          this.countDownTimerSub.unsubscribe();
        }
      });

      this.deviceService.SendDeviceChallengeAttempt(this.email).subscribe({
        next: () => {
          this.snackbar.open('Sent new code !', 'close', {
            duration: 5000,
          });
        },
        error: (err: HttpErrorResponse) => {
          let code = getBusinessErrorCode(err)

          switch (code) {
            case CODES.DEVICE_ALREADY_TRUSTED:
              this.router.navigate([`../${appPaths.LOGIN}`], {
                relativeTo: this.active,
              });
              break;

            case CODES.USER_DOES_NOT_EXIST:
              this.errorMessage = 'Invalid URL query param';
              break;

            case CODES.VALID_DEVICE_CONFIRMATION_EXISTS:
              this.errorMessage = 'Valid attempt sent wait for 1 minute';
              break;

            case CODES.DEVICE_NOT_CONFIRMED:
              this.errorMessage = "Couldn't register device for registration";
              break;

            default:
              this.errorMessage = `Unhandled error code: ${code} err: ${JSON.stringify(
                err
              )}`;
              break;
          }
        },
      });
    } else {
      // ?email value was removed from query params so send them back
      this.router.navigate([`../${appPaths.DEVICE_CHALLENGE}`], {
        relativeTo: this.active,
      });
    }
  }
}
