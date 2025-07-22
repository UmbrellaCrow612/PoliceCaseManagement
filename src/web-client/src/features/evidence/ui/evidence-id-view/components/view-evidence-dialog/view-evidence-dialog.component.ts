import { Component, inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { EvidenceService } from '../../../../../../core/evidence/services/evidence.service';
import { SafePipe } from '../../../../../../core/app/pipes/safePipe';

@Component({
  selector: 'app-view-evidence-dialog',
  imports: [MatDialogModule, MatButtonModule, SafePipe],
  templateUrl: './view-evidence-dialog.component.html',
  styleUrl: './view-evidence-dialog.component.css',
})
export class ViewEvidenceDialogComponent implements OnInit {
  private readonly evidenceService = inject(EvidenceService);

  /**
   * Prop input passed to this component of the evidence to view
   */
  private readonly evidenceId = inject<{ evidenceId: string }>(MAT_DIALOG_DATA)
    .evidenceId;

  /**
   * Holds loading state
   */
  isLoading = false;

  /**
   * Holds error state
   */
  errorMessage: string | null = null;

  /**
   * The URL used to view the document in the browser
   */
  viewUrl: string | null = null;

  /**
   * Fetches component data
   */
  fetchData() {
    if (!this.evidenceId) {
      this.errorMessage = 'Invalid evidence ID passed';
      return;
    }

    this.isLoading = true;
    this.errorMessage = null;

    this.evidenceService
      .getEvidenceInlineBrowserViewUrl(this.evidenceId)
      .subscribe({
        next: (response) => {
          this.viewUrl = response.viewUrl;
          this.isLoading = false;
        },
        error: (err) => {
          this.errorMessage = 'Failed to view document';
          this.isLoading = false;
        },
      });
  }

  ngOnInit(): void {
    this.fetchData();
  }
}
