import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { UserService } from '../../../../core/user/services/user.service';
import { DashboardHeaderComponent } from '../dashboard-header/dashboard-header.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { timer } from 'rxjs';

@Component({
  selector: 'app-dashboard-skeleton',
  imports: [RouterModule, DashboardHeaderComponent, MatProgressSpinnerModule],
  templateUrl: './dashboard-skeleton.component.html',
  styleUrl: './dashboard-skeleton.component.css',
})
export class DashboardSkeletonComponent implements OnInit {
  constructor(private userService: UserService) {}
  isLoading = true;

  ngOnInit(): void {
    this.isLoading = true;
    timer(500).subscribe(() => {
      this.isLoading = false;
    });
  }
}
