import { Component } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { IncidentTypeService } from '../../../../../core/incident-type/services/incident-type-service.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import CODES from '../../../../../core/server-responses/codes';

@Component({
  selector: 'app-system-incident-type-create-view',
  imports: [
    MatButtonModule,
    MatInputModule,
    MatFormFieldModule,
    ReactiveFormsModule,
  ],
  templateUrl: './system-incident-type-create-view.component.html',
  styleUrl: './system-incident-type-create-view.component.css',
})
export class SystemIncidentTypeCreateViewComponent {
  constructor(
    private incidentTypeService: IncidentTypeService,
    private snackBar: MatSnackBar,
    private router: Router,
    private active: ActivatedRoute
  ) {}
  createIncidentTypeForm = new FormGroup({
    name: new FormControl<string>('', [Validators.required]),
    description: new FormControl<string | null>(null),
  });

  isSendingRequest = false;
  errorMessage: string | null = null;

  onSubmit() {
    if (this.createIncidentTypeForm.valid) {
      this.isSendingRequest = true;

      this.incidentTypeService
        .create(this.createIncidentTypeForm.value)
        .subscribe({
          next: (res) => {
            this.isSendingRequest = false;
            this.snackBar.open('Created incident type', 'Close', {
              duration: 5000,
            });
            this.router.navigate(['../', `${res.id}`], {
              relativeTo: this.active,
            });
          },
          error: (err: HttpErrorResponse) => {
            this.isSendingRequest = false;

            let code = err.error.errors[0]?.code;

            switch (code) {
              case CODES.INCIDENT_TYPE_ALREADY_EXISTS:
                this.snackBar.open(
                  'A incident type with this name already exists.',
                  'Close',
                  { duration: 10000 }
                );
                break;

              default:
                this.snackBar.open('Failed to create incident type', 'Close', {
                  duration: 10000,
                });
                break;
            }
          },
        });
    }
  }
}
