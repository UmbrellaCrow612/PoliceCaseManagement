import { Component, input, OnInit } from '@angular/core';
import { IncidentTypeService } from '../../../../../../core/incident-type/services/incident-type-service.service';
import { HttpErrorResponse } from '@angular/common/http';
import { IncidentType } from '../../../../../../core/incident-type/types';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-system-incident-type-display-by-id',
  imports: [MatButtonModule],
  templateUrl: './system-incident-type-display-by-id.component.html',
  styleUrl: './system-incident-type-display-by-id.component.css',
})
// Display details about a specific incident type
export class SystemIncidentTypeDisplayByIdComponent implements OnInit {
  constructor(private incidentTypeService: IncidentTypeService) {}

  isLoading = true;
  errorMessage: string | null = null;

  /**
   * The selected incident type ID
   */
  incidentTypeId = input.required<string>();

  /**
   * Details about the selected incident type
   */
  incidentTypeDetails: IncidentType | null = null;

  fetchData() {
    this.isLoading = true;
    this.errorMessage = null;

    this.incidentTypeService
      .getIncidentTypeById(this.incidentTypeId())
      .subscribe({
        next: (value) => {
          this.incidentTypeDetails = value;
          this.isLoading = false;
        },
        error: (err: HttpErrorResponse) => {
          this.errorMessage = 'Failed';
          this.isLoading = false;
        },
      });
  }

  ngOnInit(): void {
    this.fetchData();
  }
}
