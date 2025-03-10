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
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ActivatedRoute, Router } from '@angular/router';
import formatBackendError from '../../../../core/server-responses/errors/utils/format-error';

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
    private snackBar: MatSnackBar,
    private router: Router,
    private active: ActivatedRoute
  ) {}

  isSendingRequestInProgress = false;
  deviceChallengeForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
  });

  onSubmit() {
    if (this.deviceChallengeForm.valid) {
      this.isSendingRequestInProgress = true;
      this.deviceService
        .SendDeviceChallengeAttempt(
          this.deviceChallengeForm.controls.email.value
        )
        .subscribe({
          next: (res) => {
            this.isSendingRequestInProgress = false;
            this.router.navigate(['../confirm-device-challenge'], {
              relativeTo: this.active,
            });
          },
          error: (err) => {
            this.isSendingRequestInProgress = false;
            const errorMessage = formatBackendError(err);
            this.snackBar.open(errorMessage, 'Close', {
              duration: 5000,
            });
          },
        });
    }
  }
}
