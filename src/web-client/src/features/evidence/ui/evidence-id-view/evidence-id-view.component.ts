import { Component, inject, OnInit } from '@angular/core';
import { EvidenceService } from '../../../../core/evidence/services/evidence.service';
import { Evidence } from '../../../../core/evidence/types';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { ViewEvidenceDialogComponent } from './components/view-evidence-dialog/view-evidence-dialog.component';

@Component({
  selector: 'app-evidence-id-view',
  imports: [CommonModule, MatButtonModule],
  templateUrl: './evidence-id-view.component.html',
  styleUrl: './evidence-id-view.component.css',
})
export class EvidenceIdViewComponent implements OnInit {
  private readonly evidenceService = inject(EvidenceService);
  private readonly active = inject(ActivatedRoute);
  private readonly dialog = inject(MatDialog);

  /**
   * Holds page load state
   */
  isLoading = false;

  /**
   * Holds any error state on page load
   */
  errorMessage: string | null = null;

  /**
   * The specific evidence fetched for this page i.e the /evidence/:evideceId
   */
  evidence: Evidence | null = null;

  /**
   * The ID of the pages evidence i.e the evidence to be fetched from the dyanmic URL evidences/:evidenceId
   */
  evidenceId: string | null = null;

  /**
   * Fetches inital page load data needed for the UI
   */
  fetchPageData() {
    if (!this.evidenceId) {
      this.errorMessage = 'Failed to get evidence ID from URL';
      return;
    }

    this.isLoading = true;
    this.errorMessage = null;

    this.evidenceService.getEvidenceById(this.evidenceId).subscribe({
      next: (response) => {
        this.evidence = response;
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = 'Failed to find evidence ';
        this.isLoading = false;
      },
    });
  }

  ngOnInit(): void {
    this.evidenceId = this.active.snapshot.paramMap.get('evidenceId');

    if (!this.evidenceId) {
      this.errorMessage = 'Failed to get evidence ID from URL';
      return;
    }

    this.fetchPageData();
  }

  /**
   * Runs when the view button is clicked
   */
  handleViewClicked() {
    this.dialog.open(ViewEvidenceDialogComponent, {
      data: {
        evidenceId: this.evidenceId,
      },
    });
  }
}
