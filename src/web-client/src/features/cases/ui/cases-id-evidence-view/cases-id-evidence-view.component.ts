import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CaseService } from '../../../../core/cases/services/case.service';
import { CaseEvidence } from '../../../../core/evidence/types';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthenticationService } from '../../../../core/authentication/services/authentication.service';
import { StatusCodes } from '../../../../core/http/codes/status-codes';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { LinkEvidenceDialogComponent } from './components/link-evidence-dialog/link-evidence-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-cases-id-evidence-view',
  imports: [CommonModule, MatButtonModule],
  templateUrl: './cases-id-evidence-view.component.html',
  styleUrl: './cases-id-evidence-view.component.css',
})
export class CasesIdEvidenceViewComponent implements OnInit {
  private readonly caseService = inject(CaseService);
  private readonly active = inject(ActivatedRoute);
  private readonly authService = inject(AuthenticationService);
  private readonly dialog = inject(MatDialog);

  /**
   * The ID of the case for this :caseId route
   */
  private caseId: string | null = null;

  /**
   * Holds page load state
   */
  isLoading = false;

  /**
   * Holds any erro page load state
   */
  errorMessage: string | null = null;

  /**
   * List of case evidence fetched for the given case
   */
  caseEvidenceList: CaseEvidence[] = [];
  /**
   * Fecthes all data needed for the page
   */
  fetchPageData() {
    if (!this.caseId) {
      this.errorMessage = 'Failed to fetch case ID from URL';
      return;
    }

    this.isLoading = true;
    this.errorMessage = null;

    this.caseService.getEvidence(this.caseId).subscribe({
      next: (response) => {
        this.caseEvidenceList = response;
        this.isLoading = false;
      },
      error: (err: HttpErrorResponse) => {
        this.errorMessage = 'Failed to load page';
        this.isLoading = false;

        if (err.status == StatusCodes.FORBIDDEN) {
          this.authService.UnAuthorized();
          return;
        }
      },
    });
  }

  ngOnInit(): void {
    this.caseId = this.active.snapshot.paramMap.get('caseId');
    if (!this.caseId) {
      this.errorMessage = 'Failed to fetch case ID from URL';
      return;
    }

    this.fetchPageData();
  }

  /**
   * Runs when the link button is clicked
   */
  handleLinkClick() {
    let ref = this.dialog.open(LinkEvidenceDialogComponent, {
      data: {
        caseId: this.caseId,
      },
    });

    ref.afterClosed().subscribe(() => {
      this.fetchPageData();
    });
  }
}
