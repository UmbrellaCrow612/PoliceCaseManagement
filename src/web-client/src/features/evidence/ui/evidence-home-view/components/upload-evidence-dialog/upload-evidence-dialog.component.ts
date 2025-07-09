import { Component, ElementRef, inject, ViewChild } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { FileBytePipe } from '../../../../../../core/app/pipes/fileBytePipe';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatIconModule } from '@angular/material/icon';
import { UniqueEvidenceReferenceNumberAsyncValidator } from '../../../../../../core/evidence/validators/uniqueEvidenceReferenceNumberAsyncValidator';
import { CommonModule } from '@angular/common';
import { EvidenceService } from '../../../../../../core/evidence/services/evidence.service';
import { evidenceMaxFileSizeSyncValidator } from '../../../../../../core/evidence/validators/evidenceMaxFileSizeSyncValidator';
import { formatBackendError } from '../../../../../../core/app/errors/formatError';
import { HttpEventType } from '@angular/common/http';
import { UploadEvidence } from '../../../../../../core/evidence/types';

@Component({
  selector: 'app-upload-evidence-dialog',
  imports: [
    MatDialogModule,
    MatButtonModule,
    FileBytePipe,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    MatDatepickerModule,
    MatIconModule,
    CommonModule,
  ],
  providers: [provideNativeDateAdapter()],
  templateUrl: './upload-evidence-dialog.component.html',
  styleUrl: './upload-evidence-dialog.component.css',
})
export class UploadEvidenceDialogComponent {
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;

  private readonly evidenceService = inject(EvidenceService);

  /**
   * The ref to the current open dialog of this component
   */
  private readonly dialogRef = inject(
    MatDialogRef<UploadEvidenceDialogComponent>
  );

  referenceNumberValidator = inject(
    UniqueEvidenceReferenceNumberAsyncValidator
  );

  createEvidenceForm = new FormGroup({
    name: new FormControl('', [Validators.required, Validators.minLength(5)]),
    description: new FormControl<string | null>(null, [
      Validators.maxLength(200),
    ]),
    referenceNumber: new FormControl('', {
      validators: [Validators.required, Validators.minLength(5)],
      asyncValidators: [
        this.referenceNumberValidator.validate.bind(
          this.referenceNumberValidator
        ),
      ],
      updateOn: 'blur',
    }),
    collectionDate: new FormControl<Date>(new Date(), [Validators.required]),
    file: new FormControl<File | null>(null, [
      Validators.required,
      evidenceMaxFileSizeSyncValidator(),
    ]),
  });

  /**
   * Holds any loading state
   */
  isLoading = false;

  /**
   * Holds any error message state
   */
  errorMessage: string | null = null;

  /**
   * The UI button which triggers the underlying input file
   */
  selectFileClicked() {
    this.fileInput.nativeElement.click();
  }

  /**
   * Runs when the input to select a file and a user selects a file this runs
   */
  handleFileInput(event: Event) {
    const input = event.target as HTMLInputElement;

    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      this.createEvidenceForm.controls.file.setValue(file);
      this.createEvidenceForm.controls.name.setValue(file.name);
    }
  }

  resetClicked() {
    this.createEvidenceForm.reset({
      collectionDate: new Date(),
      description: null,
      file: null,
      name: null,
      referenceNumber: null,
    });

    if (this.fileInput?.nativeElement) {
      this.fileInput.nativeElement.value = '';
    }
  }

  uploadFileClicked() {
    if (!this.createEvidenceForm.valid) {
      this.errorMessage = 'Invalid state of form';
    }

    this.isLoading = true;
    this.errorMessage = null;

    let metaData: UploadEvidence = {
      fileName: this.createEvidenceForm.controls.name.value!,
      collectionDate: this.createEvidenceForm.controls.collectionDate.value!,
      description: this.createEvidenceForm.controls.description.value!,
      referenceNumber: this.createEvidenceForm.controls.referenceNumber.value!,
      contentType: this.createEvidenceForm.controls.file.value?.type!,
      fileSize: this.createEvidenceForm.controls.file.value?.size!,
    };

    this.evidenceService.getPreSignedUploadUrl(metaData).subscribe({
      next: (response) => {
        const file: File = this.createEvidenceForm.controls.file.value!;
        const uploadUrl = response.uploadUrl;
        const evidenceId = response.evidenceId;

        this.evidenceService.uploadFileToS3(uploadUrl, file).subscribe({
          next: (event) => {
            if (event.type === HttpEventType.UploadProgress) {
              const percentDone = Math.round(
                (100 * event.loaded) / (event.total ?? 1)
              );
              console.log(`Upload progress: ${percentDone}%`);
              // Optionally show progress in the UI
            } else if (event.type === HttpEventType.Response) {
              console.log('Upload complete!');

              this.evidenceService
                .markEvidenceAsUploaded(evidenceId)
                .subscribe({
                  next: () => {
                    console.log('Evidence marked as uploaded successfully.');
                    this.dialogRef.close();
                    this.isLoading = false;
                  },
                  error: (markError) => {
                    this.errorMessage =
                      'Upload completed, but failed to mark evidence: ' +
                      formatBackendError(markError);
                    this.isLoading = false;
                  },
                });
            }
          },
          error: (uploadError) => {
            this.errorMessage =
              'Upload failed: ' + formatBackendError(uploadError);
            this.isLoading = false;
          },
        });
      },
      error: (error) => {
        this.errorMessage = formatBackendError(error);
        this.isLoading = false;
      },
    });
  }
}
