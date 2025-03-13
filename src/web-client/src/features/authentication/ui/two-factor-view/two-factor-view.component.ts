import { Component, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { ActivatedRoute, Router } from '@angular/router';
import { appPaths } from '../../../../core/app/constants/appPaths';

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
export class TwoFactorViewComponent implements OnInit {
  selectedMethod: 'sms' | 'email' | null = null;
  loginAttemptId: string | null = null;

  constructor(private router: Router, private active: ActivatedRoute) {}

  ngOnInit(): void {
    this.active.queryParamMap.subscribe((val) => {
      this.loginAttemptId = val.get('loginAttemptId');

      if (!this.loginAttemptId) {
        this.router.navigate([`../${appPaths.LOGIN}`], {
          relativeTo: this.active,
        });
      }
    });
  }

  selectMethod(method: 'sms' | 'email'): void {
    this.selectedMethod = method;
  }

  proceed(): void {
    this.router.navigate([`./${this.selectedMethod}`], {
      relativeTo: this.active,
      queryParams: { loginAttemptId: this.loginAttemptId },
    });
  }
}
