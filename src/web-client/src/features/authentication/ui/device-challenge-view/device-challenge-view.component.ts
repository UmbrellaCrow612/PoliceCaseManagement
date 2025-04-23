import { Component } from '@angular/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { DeviceService } from '../../../../core/user/device/services/device.service';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import CODES from '../../../../core/server-responses/codes';
import { appPaths } from '../../../../core/app/constants/appPaths';
import { getBusinessErrorCode } from '../../../../core/server-responses/getBusinessErrorCode';

@Component({
  selector: 'app-device-challenge-view',
  imports: [
    MatCardModule,
    MatButtonModule,
    ReactiveFormsModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './device-challenge-view.component.html',
  styleUrl: './device-challenge-view.component.css',
})
export class DeviceChallengeViewComponent {
  constructor(
    private deviceService: DeviceService,
    private router: Router,
    private active: ActivatedRoute
  ) {}

  isSendingRequestInProgress = false;
  deviceChallengeForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
  });
  errorMessage: string | null = null;
  successfullySentDeviceChallenge = false;

  onSubmit() {
    if (this.deviceChallengeForm.valid) {
      this.isSendingRequestInProgress = true;
      this.successfullySentDeviceChallenge = false;
      this.errorMessage = null;

      this.deviceService
        .SendDeviceChallengeAttempt(
          this.deviceChallengeForm.controls.email.getRawValue()!
        )
        .subscribe({
          next: () => {
            this.isSendingRequestInProgress = false;
            this.router.navigate(['../confirm-device-challenge'], {
              relativeTo: this.active,
              queryParams: {
                email: this.deviceChallengeForm.controls.email.getRawValue()!,
              },
              queryParamsHandling: 'merge',
            });
          },
          error: (err: HttpErrorResponse) => {
            this.isSendingRequestInProgress = false;

            let code = getBusinessErrorCode(err)

            switch (code) {
              case CODES.DEVICE_ALREADY_TRUSTED:
                this.router.navigate([`../${appPaths.LOGIN}`], {
                  relativeTo: this.active,
                });
                break;

              case CODES.USER_DOES_NOT_EXIST:
                this.successfullySentDeviceChallenge = true;
                break;

              case CODES.VALID_DEVICE_CONFIRMATION_EXISTS:
                this.errorMessage = 'Valid attempt sent wait for 2 minutes';
                break;

              case CODES.DEVICE_NOT_CONFIRMED:
                this.errorMessage = "Couldn't register device for registration";
                break;

              default:
                break;
            }
          },
        });
    }
  }
}
