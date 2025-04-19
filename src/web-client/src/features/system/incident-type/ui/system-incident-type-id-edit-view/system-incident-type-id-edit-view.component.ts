import { Component, OnInit } from '@angular/core';
import { IncidentTypeService } from '../../../../../core/incident-type/services/incident-type-service.service';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { CanComponentDeactivate } from '../../../../../core/app/guards/canDeactivateGuard';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-system-incident-type-id-edit-view',
  imports: [
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    ReactiveFormsModule,
  ],
  templateUrl: './system-incident-type-id-edit-view.component.html',
  styleUrl: './system-incident-type-id-edit-view.component.css',
})
export class SystemIncidentTypeIdEditViewComponent
  implements OnInit, CanComponentDeactivate
{
  constructor(
    private incidentTypeService: IncidentTypeService,
    private active: ActivatedRoute
  ) {}

  isLoading = true;
  errorMessage: string | null = null;
  incidentTypeId: string | null = null;
  editIncidentTypeForm = new FormGroup({
    name: new FormControl('', [Validators.required]),
    description: new FormControl<string | null>(null),
  });

  fetchData() {
    if (!this.incidentTypeId) {
      return;
    }

    this.isLoading = true;
    this.errorMessage = null;
    this.editIncidentTypeForm.reset();
    this.editIncidentTypeForm.disable();

    this.incidentTypeService
      .getIncidentTypeById(this.incidentTypeId)
      .subscribe({
        next: (incidentType) => {
          this.editIncidentTypeForm.patchValue({
            name: incidentType.name,
            description: incidentType.description,
          });
          this.editIncidentTypeForm.enable();
          this.isLoading = false;
        },
        error: () => {
          this.errorMessage = 'Failed to fetch incident type.';
          this.isLoading = false;
        },
      });
  }

  ngOnInit(): void {
    this.incidentTypeId = this.active.snapshot.paramMap.get('incidentTypeId');
    if (!this.incidentTypeId) {
      this.errorMessage = 'Failed to get id from URL';
      return;
    }

    this.fetchData();
  }

  canDeactivate(): boolean | Observable<boolean> {
    if (this.editIncidentTypeForm.dirty) {
      return confirm(
        'Are you sure you want to leave without saving your changes'
      );
    }
    return true;
  }

  onSubmit() {
    if (this.editIncidentTypeForm.valid) {
      this.isLoading = true;

      this.incidentTypeService
        .update({
          id: this.incidentTypeId,
          ...this.editIncidentTypeForm.value,
        })
        .subscribe({
          next: () => {
            this.fetchData();
          },
          error: () => {
            this.errorMessage = `Failed`;
            this.isLoading = false;
          },
        });
    }
  }
}
