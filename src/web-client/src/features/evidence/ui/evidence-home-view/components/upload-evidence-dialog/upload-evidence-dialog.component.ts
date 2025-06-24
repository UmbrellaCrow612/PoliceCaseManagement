import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { FileBytePipe } from '../../../../../../core/app/pipes/fileBytePipe';
import {
  Validator_maxFileSize,
  Validator_password,
} from '../../../../../../core/app/validators/controls';

@Component({
  selector: 'app-upload-evidence-dialog',
  imports: [MatDialogModule, MatButtonModule, FileBytePipe],
  templateUrl: './upload-evidence-dialog.component.html',
  styleUrl: './upload-evidence-dialog.component.css',
})
export class UploadEvidenceDialogComponent {
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;

  createEvidenceForm = new FormGroup({
    name: new FormControl('', [Validators.required, Validators.minLength(5)]),
    description: new FormControl<string | null>(null),
    referenceNumber: new FormControl('', [Validators.required]),
    fileName: new FormControl('', [
      Validators.required,
      Validators.minLength(5),
    ]),
    collectionDate: new FormControl<Date>(new Date(), [Validators.required]),
    file: new FormControl<File | null>(null, [
      Validators.required,
      Validator_maxFileSize(5e6),
    ]),
  });

  /**
   * Holds any loading state
   */
  isLoading = false;

  /**
   * Holds any error message state
   */
  errorMessage = '';

  selectFileClicked() {
    this.fileInput.nativeElement.click();
  }

  handleFileInput(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      this.createEvidenceForm.controls.file.setValue(file);
    }
  }
}
