import { Component, inject, OnInit } from '@angular/core';
import { UserService } from '../../../../core/user/services/user.service';
import { CaseService } from '../../../../core/cases/services/case.service';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { SearchUsersSelectComponent } from '../../../../core/user/components/search-users-select/search-users-select.component';
import { CommonModule } from '@angular/common';
import { RestrictedUser } from '../../../../core/user/type';

@Component({
  selector: 'app-cases-me-view',
  imports: [SearchUsersSelectComponent, ReactiveFormsModule, CommonModule],
  templateUrl: './cases-me-view.component.html',
  styleUrl: './cases-me-view.component.css',
})
export class CasesMeViewComponent implements OnInit {
  ngOnInit(): void {}
  private currentUser = inject(UserService).USER;
  private readonly caseService = inject(CaseService);

  form = new FormGroup({
    selectedUser: new FormControl<RestrictedUser | null>(null, [
      Validators.required,
    ]),
    selectedUserTwo: new FormControl<RestrictedUser | null>(null, [Validators.required])
  });

  isLoading = true;
  error: string | null = null;

  searchCasesForm = new FormGroup({
    status: new FormControl<string | null>(null),
    priority: new FormControl<string | null>(null),
  });

  filterClicked() {
    this.fetchData();
  }

  fetchData() {
    this.isLoading = true;
    this.error = null;

    this.caseService.searchCasesWithPagination({
      status: this.searchCasesForm.controls.status.getRawValue(),
      priority: this.searchCasesForm.controls.priority.getRawValue(),
      createdById: this.currentUser?.id,
      assignedUserIds: [this.currentUser?.id!],
    });
  }
}
