import { Component, output } from '@angular/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { IncidentTypeService } from '../../../../../../core/incident-type/services/incident-type-service.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-system-incident-type-create',
  imports: [
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  templateUrl: './system-incident-type-create.component.html',
  styleUrl: './system-incident-type-create.component.css',
})
export class SystemIncidentTypeCreateComponent {
  constructor(
    private IncidentTypeService: IncidentTypeService,
    private snackBar: MatSnackBar
  ) {}

  createIncidentTypeForm = new FormGroup({
    name: new FormControl('', [Validators.required]),
    description: new FormControl(null),
  });

  isSendingRequest = false;
  errorMessage: string | null = null;

  /**
   * Fired off when a incident type is created successfully
   */
  createdIncidentTypeEvent = output<void>();

  onSubmit() {
    if (this.createIncidentTypeForm.valid) {
      this.isSendingRequest = true;
      this.errorMessage = null;

      this.IncidentTypeService.create(
        this.createIncidentTypeForm.value
      ).subscribe({
        next: () => {
          this.snackBar.open('created incident type', 'Close', {
            duration: 10000,
          });
          this.createdIncidentTypeEvent.emit();
          this.isSendingRequest = false;
        },
        error: (err: HttpErrorResponse) => {
          this.snackBar.open(
            `Description: ${err.error.errors[0]?.message}`,
            'Close',
            {
              duration: 10000,
            }
          );
          this.isSendingRequest = false;
        },
      });
    }
  }
}
