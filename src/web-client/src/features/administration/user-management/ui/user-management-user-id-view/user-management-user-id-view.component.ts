import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../../../../core/user/services/user.service';
import { User } from '../../../../../core/user/type';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { forkJoin } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-user-management-user-id-view',
  imports: [MatButtonModule, MatProgressSpinnerModule],
  templateUrl: './user-management-user-id-view.component.html',
  styleUrl: './user-management-user-id-view.component.css',
})
export class UserManagementUserIdViewComponent implements OnInit {
  userId: string | null = null;
  user: User | null = null;
  roles: string[] = [];
  isLoading = true;
  errorMessage: string | null = null;

  constructor(
    private active: ActivatedRoute,
    private router: Router,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.userId = this.active.snapshot.paramMap.get('userId');
    this.fetchData();
  }

  fetchData() {
    this.isLoading = true;
    this.errorMessage = null;

    forkJoin({
      user: this.userService.getUserById({ userId: this.userId! }),
      rolesResponse: this.userService.getUserRolesById({
        userId: this.userId!,
      }),
    }).subscribe({
      next: (response) => {
        this.user = response.user;
        this.roles = response.rolesResponse.roles;
        this.isLoading = false;
      },
      error: (err: HttpErrorResponse) => {
        this.errorMessage = `Failed to fetch user data code: ${err.error[0]?.code}`;
        this.isLoading = false;
      },
    });
  }

  editDetailsClicked() {
    this.router.navigate(['edit/details'], { relativeTo: this.active });
  }

  editRolesClicked() {
    this.router.navigate(['edit/roles'], { relativeTo: this.active });
  }
}
