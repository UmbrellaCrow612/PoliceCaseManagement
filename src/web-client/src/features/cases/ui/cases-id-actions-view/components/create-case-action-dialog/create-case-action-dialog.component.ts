import { CommonModule } from '@angular/common';
import { Component, inject, input } from '@angular/core';
import {
  FormControl,
  FormGroup,
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
import {
  CaseAction,
  CreateCase,
  CreateCaseAction,
} from '../../../../../../core/cases/type';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { timer } from 'rxjs';
import { CaseActionDetailsComponent } from '../case-action-details/case-action-details.component';

interface CreateCaseActionDialogData {
  /**
   * The parent component has validated this case exists and passes the case ID through
   */
  caseId: string;
}

@Component({
  selector: 'app-create-case-action-dialog',
  imports: [
    MatDialogModule,
    MatButtonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    CommonModule,
  ],
  templateUrl: './create-case-action-dialog.component.html',
  styleUrl: './create-case-action-dialog.component.css',
})
export class CreateCaseActionDialogComponent {
  constructor(
    private caseService: CaseService,
    private snackBar: MatSnackBar
  ) {}
  readonly dialogRef = inject(MatDialogRef<CreateCaseActionDialogComponent>);
  data: CreateCaseActionDialogData = inject(MAT_DIALOG_DATA);
  createCaseActionForm = new FormGroup({
    description: new FormControl('', [Validators.required]),
    notes: new FormControl<string | null>(null),
  });

  isLoading = false;
  onSubmit() {
    if (this.createCaseActionForm.valid) {
      this.isLoading = true;

      let action: CreateCaseAction = {
        description: this.createCaseActionForm.controls.description.value!,
        notes: this.createCaseActionForm.controls.notes.value,
      };

      this.caseService.addCaseAction(this.data.caseId, action).subscribe({
        next: () => {
          this.isLoading = false;
          this.snackBar.open('Created case action', 'Close', {
            duration: 5000,
          });
          timer(250).subscribe(() => {
            this.dialogRef.close();
          });
        },
        error: () => {},
      });
    }
  }
}
