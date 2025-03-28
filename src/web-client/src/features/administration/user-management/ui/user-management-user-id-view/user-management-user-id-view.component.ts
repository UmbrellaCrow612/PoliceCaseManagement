import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../../../../core/user/services/user.service';
import { User } from '../../../../../core/user/type';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-user-management-user-id-view',
  imports: [MatButtonModule, MatProgressSpinnerModule],
  templateUrl: './user-management-user-id-view.component.html',
  styleUrl: './user-management-user-id-view.component.css',
})
export class UserManagementUserIdViewComponent implements OnInit {
  userId: string | null = null;
  user: User | null = null;
  userNotFound = false;
  isLoadingData = true;

  constructor(
    private active: ActivatedRoute,
    private router: Router,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.userId = this.active.snapshot.paramMap.get('userId');
    if (!this.userId) {
      this.userNotFound = true;
    }
    this.isLoadingData = true;

    this.userService.getUserById({ userId: this.userId! }).subscribe({
      next: (val) => {
        this.userNotFound = false;
        this.user = val;
        this.isLoadingData = false;
      },
      error: () => {
        this.userNotFound = true;
        this.isLoadingData = false;
      },
    });
  }

  editClick() {
    this.router.navigate(['edit'], { relativeTo: this.active });
  }

  // if user then get there roles from endpoint and also department etc
}
