import { Component, inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { AddPersonToCaseDialogComponent } from './components/add-person-to-case-dialog/add-person-to-case-dialog.component';
import { ActivatedRoute } from '@angular/router';
import { CasePerson } from '../../../../core/people/types';
import { CaseService } from '../../../../core/cases/services/case.service';
import { HttpErrorResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-cases-id-people-view',
  imports: [MatButtonModule, CommonModule],
  templateUrl: './cases-id-people-view.component.html',
  styleUrl: './cases-id-people-view.component.css',
})
export class CasesIdPeopleViewComponent implements OnInit {
  private readonly dialog = inject(MatDialog);
  private readonly active = inject(ActivatedRoute);
  private readonly caseService = inject(CaseService);

  /**
   * List of people linked ot the given case
   */
  casePeople: CasePerson[] | null = null;

  /**
   * The ID of the current case in the UI - extracted from the URL /cases/:caseId
   */
  private caseId: string | null = null;

  /**
   * Holds any error state of the UI
   */
  error: string | null = null;

  /**
   * Holds loading state for UI load
   */
  isLoading = false;

  /**
   * Runs when add people btn is clicked
   */
  handleAddClicked() {
    this.dialog.open(AddPersonToCaseDialogComponent, {
      data: {
        caseId: this.caseId,
      },
    });
  }

  ngOnInit(): void {
    this.caseId = this.active.snapshot.paramMap.get('caseId');
    if (!this.caseId) {
      this.error = 'Failed ot get caseId from URL';
      return;
    }

    this.error = null;
    this.isLoading = true;

    this.caseService.getCasePeople(this.caseId).subscribe({
      next: (response) => {
        this.casePeople = response;
        this.isLoading = false;
      },
      error: (err: HttpErrorResponse) => {
        this.error = 'Failed to load case people';
        this.isLoading = false;
      },
    });
  }
}
