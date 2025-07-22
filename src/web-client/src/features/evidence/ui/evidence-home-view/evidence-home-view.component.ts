import { Component, inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { UploadEvidenceDialogComponent } from './components/upload-evidence-dialog/upload-evidence-dialog.component';
import { MatButtonModule } from '@angular/material/button';
import { EvidenceService } from '../../../../core/evidence/services/evidence.service';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import {
  Evidence,
  EvidenceOrderByNames,
} from '../../../../core/evidence/types';
import { PaginatedResult } from '../../../../core/app/type';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CommonModule } from '@angular/common';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatSelectModule } from '@angular/material/select';
import { RestrictedUser } from '../../../../core/user/type';
import { SearchUsersSelectComponent } from '../../../../core/user/components/search-users-select/search-users-select.component';
import { EvidenceGridListComponent } from '../../../../core/evidence/components/evidence-grid-list/evidence-grid-list.component';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-evidence-home-view',
  imports: [
    MatButtonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    CommonModule,
    MatDatepickerModule,
    MatSelectModule,
    SearchUsersSelectComponent,
    EvidenceGridListComponent,
  ],
  templateUrl: './evidence-home-view.component.html',
  styleUrl: './evidence-home-view.component.css',
  providers: [provideNativeDateAdapter()],
})
export class EvidenceHomeViewComponent {
  private readonly dialog = inject(MatDialog);
  private readonly evidenceService = inject(EvidenceService);
  private readonly router = inject(Router);
  private readonly active = inject(ActivatedRoute);

  /**
   * Local copy of EvidenceOrderByNames
   */
  copy_EvidenceOrderByNames = EvidenceOrderByNames;

  /**
   * Form group used to search for evidences in the system
   */
  searchEvidenceForm = new FormGroup({
    referenceNumber: new FormControl<string | null>(null),
    fileName: new FormControl<string | null>(null),
    contentType: new FormControl<string | null>(null),
    uploadedAt: new FormControl<Date | null>(null),
    collectionDate: new FormControl<Date | null>(null),
    orderBy: new FormControl<number | null>(null),
    uploadedBy: new FormControl<RestrictedUser | null>(null),
  });

  /**
   * Holds loading state
   */
  isLoading = false;

  /**
   * Holds any error state
   */
  errorMessage: string | null = null;

  /**
   * List of evidence items macthing the search term fetched from the backend in a paginated format
   */
  searchedEvidencePaginated: PaginatedResult<Evidence> | null = null;

  /**
   * Runs when the search button is clciked
   * @param pageNumber The page number to fetch - by default it fetches the first page number
   */
  searchClicked(pageNumber: number = 1) {
    this.isLoading = true;
    this.errorMessage = null;

    this.evidenceService
      .searchEvidence({
        collectionDate: this.searchEvidenceForm.controls.collectionDate.value,
        contentType: this.searchEvidenceForm.controls.contentType.value,
        fileName: this.searchEvidenceForm.controls.fileName.value,
        orderBy: this.searchEvidenceForm.controls.orderBy.value,
        pageNumber: pageNumber,
        pageSize: 20,
        referenceNumber: this.searchEvidenceForm.controls.referenceNumber.value,
        uploadedAt: this.searchEvidenceForm.controls.uploadedAt.value,
        uploadedById:
          this.searchEvidenceForm.controls.uploadedBy.value?.id ?? null,
      })
      .subscribe({
        next: (response) => {
          this.searchedEvidencePaginated = response;
          this.isLoading = false;
        },
        error: () => {
          this.errorMessage = 'Failed to fetch';
          this.isLoading = false;
        },
      });
  }

  /**
   * Runs when the upload button is clicked
   */
  uploadClicked() {
    this.dialog.open(UploadEvidenceDialogComponent, {
      width: '100%',
      maxWidth: '600px',
      height: '100%',
      maxHeight: '600px',
    });
  }

  /**
   * Runs when evidence grid item is selcted
   * @param item The evidence selected
   */
  handleEvidenceItemClicked(item: Evidence) {
    this.router.navigate([item.id], {
      relativeTo: this.active,
    });
  }
}
