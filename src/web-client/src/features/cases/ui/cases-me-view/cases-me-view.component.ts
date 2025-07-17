import { Component, inject, OnInit } from '@angular/core';
import { UserService } from '../../../../core/user/services/user.service';
import { CaseService } from '../../../../core/cases/services/case.service';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CasePrioritySelectComponent } from '../../../../core/cases/components/case-priority-select/case-priority-select.component';
import { CaseStatusSelectComponent } from '../../../../core/cases/components/case-status-select/case-status-select.component';
import { CasePagedResult } from '../../../../core/cases/type';
import { formatBackendError } from '../../../../core/app/errors/formatError';
import { MatButtonModule } from '@angular/material/button';
import { CaseGridListComponent } from '../../../../core/cases/components/case-grid-list/case-grid-list.component';

@Component({
  selector: 'app-cases-me-view',
  imports: [
    ReactiveFormsModule,
    CommonModule,
    CasePrioritySelectComponent,
    CaseStatusSelectComponent,
    MatButtonModule,
    CaseGridListComponent,
  ],
  templateUrl: './cases-me-view.component.html',
  styleUrl: './cases-me-view.component.css',
})
export class CasesMeViewComponent implements OnInit {
  ngOnInit(): void {
    this.searchCasesForm.valueChanges.subscribe(() => {
      this.fetchData();
    });

    this.fetchData();
  }

  private currentUser = inject(UserService).USER;
  private readonly caseService = inject(CaseService);

  isLoading = true;
  error: string | null = null;

  /**
   * Form used to filter my cases
   */
  searchCasesForm = new FormGroup({
    priority: new FormControl<string | null>(null),
    status: new FormControl<string | null>(null),
  });
  /**
   * List of cases fetched
   */
  searchCasesResult: CasePagedResult | null = null;

  nextClicked() {
    this.fetchData({
      pageNumber: this.searchCasesResult?.hasNextPage
        ? this.searchCasesResult.pagination.currentPage + 1
        : null,
    });
  }

  previousClicked() {
    this.fetchData({
      pageNumber: this.searchCasesResult?.hasPreviousPage
        ? this.searchCasesResult.pagination.currentPage - 1
        : null,
    });
  }
  fetchData(options: Partial<{ pageNumber: number | null }> = {}) {
    this.isLoading = true;
    this.error = null;
    this.searchCasesResult = null;

    this.caseService
      .searchCasesWithPagination({
        status: this.searchCasesForm.controls.status.value,
        priority: this.searchCasesForm.controls.priority.value,
        assignedUserIds: [this.currentUser?.id!],
        pageNumber: options.pageNumber,
      })
      .subscribe({
        next: (result) => {
          this.searchCasesResult = result;
          this.isLoading = false;
        },
        error: (err) => {
          this.error = formatBackendError(err);
          this.isLoading = false;
        },
      });
  }
}
