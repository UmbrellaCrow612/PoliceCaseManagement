import { Component, ElementRef, inject, ViewChild } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { FileBytePipe } from '../../../../../../core/app/pipes/fileBytePipe';
import { Validator_maxFileSize } from '../../../../../../core/app/validators/controls';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatIconModule } from '@angular/material/icon';
import { UniqueEvidenceReferenceNumberAsyncValidator } from '../../../../../../core/evidence/validators/uniqueEvidenceReferenceNumberAsyncValidator';

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
  ],
  providers: [provideNativeDateAdapter()],
  templateUrl: './upload-evidence-dialog.component.html',
  styleUrl: './upload-evidence-dialog.component.css',
})
export class UploadEvidenceDialogComponent {
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;

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
  errorMessage = null;

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
}
