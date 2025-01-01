import { MatFormFieldModule } from '@angular/material/form-field';
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from '../../../../core/authentication/services/authentication.service';
import { SmsCodeRequest } from '../../../../core/authentication/types';
import { MatSnackBar } from '@angular/material/snack-bar';
import formatBackendError from '../../../../core/errors/utils/format-error';

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

  ngOnInit(): void {
    this.sendCode();
  }

  private sendCode() {
    this.SmsCodeRequest.loginAttemptId =
      this.active.snapshot.queryParams['loginAttemptId'];

    if (
      typeof this.SmsCodeRequest.loginAttemptId !== 'string' ||
      this.SmsCodeRequest.loginAttemptId.trim() === ''
    ) {
      this.router.navigate(['../../login'], { relativeTo: this.active });
      return;
    }

    this.authService.SendSmsCode(this.SmsCodeRequest).subscribe({
      next: (config) => {
        console.log(`SMS code sent successfully code : ${config.code}`);
      },
      error: (err) => {
        let errorMessage = formatBackendError(err);
        this.snackBar.open(errorMessage, 'close', { duration: 7000 });
      },
    });
  }

  SmsCodeRequest: SmsCodeRequest = {
    loginAttemptId: '',
  };

  smsForm = new FormGroup({
    code: new FormControl('', [Validators.required]),
  });

  onSubmit() {
    if (this.smsForm.valid) {
      console.log();
    }
  }
}
