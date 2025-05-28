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
      }
    }
  }

  isUploading = false;
  submit() {
    if (this.selectedFile && !this.errorMessage) {
      this.isUploading = true;

      this.caseService
        .uploadAttachment(this.caseId!, this.selectedFile)
        .subscribe({
          next: () => {
            this.dialogRef.close();
          },
          error: (err) => {
            this.errorMessage = formatBackendError(err);
            this.isUploading = false;
          },
        });
    }
  }
}
