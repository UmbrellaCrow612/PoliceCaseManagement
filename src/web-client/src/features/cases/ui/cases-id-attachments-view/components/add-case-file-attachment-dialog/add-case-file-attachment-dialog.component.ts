import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import {
  FormControl,
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
import { CaseService } from '../../../../../../core/cases/services/case.service';
import { formatBackendError } from '../../../../../../core/app/errors/formatError';
import { catchError, EMPTY, finalize, switchMap, tap } from 'rxjs';

@Component({
  selector: 'app-add-case-file-attachment-dialog',
  imports: [
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    CommonModule,
    ReactiveFormsModule,
  ],
  templateUrl: './add-case-file-attachment-dialog.component.html',
  styleUrl: './add-case-file-attachment-dialog.component.css',
})
export class AddCaseFileAttachmentDialogComponent implements OnInit {
  constructor(private readonly caseService: CaseService) {}
  ngOnInit(): void {
    if (!this.caseId) {
      this.errorMessage = 'Case ID not passed to dialog';
    }
    return;
  }
  fileNameInput = new FormControl<string | null>(null, [
    Validators.required,
    Validators.minLength(5),
  ]);

  selectedFile: File | null = null;

  errorMessage: string = '';

  acceptedFileTypes =
    '.png,.jpg,.jpeg,.gif,.bmp,.pdf,.doc,.docx,.xls,.xlsx,.txt';

  maxFileSizeBytes = 5 * 1024 * 1024; // 5 MB

  caseId: string | null = inject<{ caseId: string }>(MAT_DIALOG_DATA).caseId;

  readonly dialogRef = inject(
    MatDialogRef<AddCaseFileAttachmentDialogComponent>
  );

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input?.files?.length) {
      const file = input.files[0];

      if (file.size > this.maxFileSizeBytes) {
        this.errorMessage = 'File size exceeds 5MB limit.';
        this.selectedFile = null;
      } else {
        this.selectedFile = file;
        this.errorMessage = '';
        this.fileNameInput.setValue(file.name);
      }
    }
  }

  isUploading = false;
  submit() {
    if (this.selectedFile && !this.errorMessage && this.fileNameInput.valid) {
      this.isUploading = true;
      this.caseService
        .getPreSignedUrlForCaseAttachmentFile(
          this.caseId!,
          this.selectedFile,
          this.fileNameInput.value!
        )
        .pipe(
          switchMap((response) =>
            this.caseService
              .uploadUsingPreSignedUrl(response.uploadUrl, this.selectedFile!)
              .pipe(
                switchMap(() =>
                  this.caseService.confirmUploadOfCaseAttachmentFile(
                    response.fileId
                  )
                ),
                tap(() => {
                  this.dialogRef.close();
                })
              )
          ),
          catchError((err) => {
            if (err?.message?.includes('invalid state')) {
              this.errorMessage = 'Server error file in invalid state';
            } else if (err?.message?.includes('upload')) {
              this.errorMessage = 'Failed to upload amazon';
            } else {
              this.errorMessage = 'Failed to upload file';
            }
            this.isUploading = false;
            return EMPTY;
          }),
          finalize(() => {
            this.isUploading = false;
          })
        )
        .subscribe();
    } else {
      this.errorMessage = 'Invalid state';
    }
  }
}
