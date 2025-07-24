import { Component, inject } from '@angular/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import {
  MAT_DIALOG_DATA,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { EvidenceService } from '../../../../../../core/evidence/services/evidence.service';
import { Evidence } from '../../../../../../core/evidence/types';
import { PaginatedResult } from '../../../../../../core/app/type';
import { MatListModule } from '@angular/material/list';
import { CaseService } from '../../../../../../core/cases/services/case.service';

@Component({
  selector: 'app-link-evidence-dialog',
  imports: [
    MatDialogModule,
    MatButtonModule,
    ReactiveFormsModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatListModule,
  ],
  templateUrl: './link-evidence-dialog.component.html',
  styleUrl: './link-evidence-dialog.component.css',
})
export class LinkEvidenceDialogComponent {
  private readonly evidenceService = inject(EvidenceService);
  private readonly caseService = inject(CaseService);
  private readonly data: { caseId: string } = inject(MAT_DIALOG_DATA);
  private readonly ref = inject(MatDialogRef<LinkEvidenceDialogComponent>);
  /**
   * Contaisn all search fields for evidence
   */
  searchEvidenceForm = new FormGroup({
    refNumber: new FormControl('', [Validators.required]),
  });

  /**
   * List of matching evidence for the search term
   */
  EvidenceList: PaginatedResult<Evidence> | null = null;

  /**
   * Holds loading state
   */
  isLoading = false;

  /**
   * Holds error message
   */
  errorMessage: string | null = null;

  /**
   * A selected evidence to link
   */
  selectedEvidenceControl = new FormControl<Evidence[] | null>(null, [
    Validators.required,
  ]);

  /**
   * Holds state for when a evidence item is being added
   */
  isAdding = false;

  /**
   * Runs when search is clicked
   */
  searchClicked() {
    if (this.searchEvidenceForm.valid) {
      this.isLoading = true;
      this.errorMessage = null;
      this.EvidenceList = null;

      this.evidenceService
        .searchEvidence({
          collectionDate: null,
          contentType: null,
          fileName: null,
          orderBy: null,
          pageNumber: 1,
          pageSize: 20,
          referenceNumber: this.searchEvidenceForm.controls.refNumber.value,
          uploadedAt: null,
          uploadedById: null,
        })
        .subscribe({
          next: (response) => {
            this.EvidenceList = response;
            this.isLoading = false;
          },
          error: (err) => {
            this.errorMessage = 'Failed to fetch evidence';
            this.isLoading = false;
          },
        });
    }
  }

  /**
   * Handles when add evidence is clicked
   */
  handleAddClicked() {
    if (this.selectedEvidenceControl.valid) {
      let selectedEvidence: Evidence | undefined =
        this.selectedEvidenceControl.value?.[0];

      if (!selectedEvidence || !this.data.caseId) {
        this.errorMessage = 'No selected evidence or caseId';
      }

      this.isAdding = true;
      this.errorMessage = null;

      this.caseService
        .addEvidence(this.data.caseId, selectedEvidence?.id!)
        .subscribe({
          next: () => {
            this.ref.close();
          },
          error: () => {
            this.errorMessage = 'Failed to attach';
            this.isAdding = false;
          },
        });
    }
  }
}
