import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { forkJoin, timer } from 'rxjs';
import { CaseActionService } from '../../../../../../core/cases/services/case-action.service';
import { CaseAction } from '../../../../../../core/cases/type';
import { RestrictedUser } from '../../../../../../core/user/type';

@Component({
  selector: 'app-case-action-dialog-details',
  imports: [MatDialogModule, CommonModule, MatButtonModule],
  templateUrl: './case-action-dialog-details.component.html',
  styleUrl: './case-action-dialog-details.component.css',
})
export class CaseActionDialogDetailsComponent implements OnInit {
  constructor(private caseActionService: CaseActionService) {}
  ngOnInit(): void {
    timer(2500).subscribe(() => {
      this.isDeleteButtonDiabled = false;
    });

    if (!this.actionId) {
      this.error = 'Failed to pass ID to child';
      return;
    }
    this.fetchData();
  }
  /**
   * Id of the action clciked passed from the parent
   */
  readonly actionId = inject<{ id: string }>(MAT_DIALOG_DATA).id;

  isLoading = true;
  error: string | null = null;
  isDeleteButtonDiabled = true;

  caseActionDetails: CaseAction | null = null;
  createdByDetails: RestrictedUser | null = null;

  fetchData() {
    this.isLoading = true;
    this.error = null;

    const caseActionDetailsRequest = this.caseActionService.getCaseActionById(
      this.actionId
    );

    forkJoin([caseActionDetailsRequest]).subscribe({
      next: ([caseAction]) => {
        this.caseActionDetails = caseAction;

        this.isLoading = false;
      },
      error: (err) => {
        this.error = err;
        this.isLoading = false;
      },
    });
  }
}
