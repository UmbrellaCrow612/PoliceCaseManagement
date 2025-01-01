import { MatFormFieldModule } from '@angular/material/form-field';
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
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { ActivatedRoute, Router } from '@angular/router';

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
export class TwoFactorSmsViewComponent {
  constructor(private router: Router, private active: ActivatedRoute) {}

  loginAttemptId: string | null = null;
  email: string | null = null;

  smsForm = new FormGroup({
    code: new FormControl('', [Validators.required]),
  });

  onSubmit() {
    this.loginAttemptId = this.active.snapshot.queryParams['loginAttemptId'];
    this.email = this.active.snapshot.queryParams['email'];
    if (
      typeof this.loginAttemptId !== 'string' ||
      this.loginAttemptId.trim() === '' ||
      typeof this.email !== 'string' ||
      this.email.trim() === ''
    ) {
      this.router.navigate(['../../login'], { relativeTo: this.active });
      return;
    }

    if (this.smsForm.valid) {
      console.log(`Code: ${this.smsForm.value.code} email: ${this.email} loginAttemptId: ${this.loginAttemptId}`);
    }
  }
}
