import { Component, OnInit } from '@angular/core';
import { CaseService } from '../../../../core/cases/services/case.service';
import { ActivatedRoute } from '@angular/router';
import { CasePermission } from '../../../../core/cases/type';
import { formatBackendError } from '../../../../core/app/errors/formatError';
import { CommonModule } from '@angular/common';
import { BackNavigationButtonComponent } from '../../../../core/components/back-navigation-button/back-navigation-button.component';
import { CasePermissionItemComponent } from './components/case-permission-item/case-permission-item.component';

@Component({
  selector: 'app-cases-id-permissions-view',
  imports: [
    CommonModule,
    BackNavigationButtonComponent,
    CasePermissionItemComponent,
  ],
  templateUrl: './cases-id-permissions-view.component.html',
  styleUrl: './cases-id-permissions-view.component.css',
})
export class CasesIdPermissionsViewComponent implements OnInit {
  constructor(
    private readonly caseService: CaseService,
    private readonly active: ActivatedRoute
  ) {}
  ngOnInit(): void {
    this.caseId = this.active.snapshot.paramMap.get('caseId');

    this.fetchData();
  }

  permissions: CasePermission[] = [];

  caseId: string | null = null;

  isLoading = false;
  error: string | null = null;

  fetchData() {
    if (!this.caseId) {
      this.error = 'Case ID not extracted from URL';
      return;
    }

    this.isLoading = true;
    this.error = null;

    this.caseService.getPermissions(this.caseId).subscribe({
      next: (perms) => {
        this.permissions = perms;
        this.isLoading = false;
      },
      error: (err) => {
        this.error = formatBackendError(err);
        this.isLoading = false;
      },
    });
  }

  // we dont use save changes state stuff as it makes it more compolex for this secernio instead just fire off request as they change values
  // just use toasts to convey information
  // disable the row as one slide toggle - might need to extract it into own component for that
}
