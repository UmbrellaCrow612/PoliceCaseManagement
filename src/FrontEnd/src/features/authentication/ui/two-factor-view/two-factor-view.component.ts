import { Component } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-two-factor-view',
  imports: [
    MatCardModule,
    MatSelectModule,
    MatIconModule,
    MatListModule,
    MatButtonModule,
  ],
  templateUrl: './two-factor-view.component.html',
  styleUrl: './two-factor-view.component.css',
})
export class TwoFactorViewComponent {
  selectedMethod: 'sms' | 'email' | null = null;
  loginAttemptId: string | null = null;

  constructor(
    private snackBar: MatSnackBar,
    private router: Router,
    private active: ActivatedRoute
  ) {}

  selectMethod(method: 'sms' | 'email'): void {
    this.selectedMethod = method;
  }

  proceed(): void {
    this.loginAttemptId = this.active.snapshot.queryParams['loginAttemptId'];
    if (
      typeof this.loginAttemptId !== 'string' ||
      this.loginAttemptId.trim() === ''
    ) {
      this.router.navigate(['../login'], { relativeTo: this.active });
      return;
    }

    this.router.navigate([`./${this.selectedMethod}`], {
      relativeTo: this.active,
      queryParams: { loginAttemptId: this.loginAttemptId },
    });
  }
}
