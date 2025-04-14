import { Component, OnInit } from '@angular/core';
import { IncidentTypeService } from '../../../../core/incident-type/services/incident-type-service.service';
import { IncidentType } from '../../../../core/incident-type/types';
import { HttpErrorResponse } from '@angular/common/http';
import {
  IncidentTypeClickedEvent,
  SystemIncidentTypeDisplayAllComponent,
} from './components/system-incident-type-display-all/system-incident-type-display-all.component';
import { SystemIncidentTypeDisplayByIdComponent } from './components/system-incident-type-display-by-id/system-incident-type-display-by-id.component';
import { SystemIncidentTypeCreateComponent } from './components/system-incident-type-create/system-incident-type-create.component';

@Component({
  selector: 'app-system-incident-type-view',
  imports: [
    SystemIncidentTypeDisplayAllComponent,
    SystemIncidentTypeDisplayByIdComponent,
    SystemIncidentTypeCreateComponent,
  ],
  templateUrl: './system-incident-type-view.component.html',
  styleUrl: './system-incident-type-view.component.css',
})
export class SystemIncidentTypeViewComponent implements OnInit {
  constructor(private incidentTypeService: IncidentTypeService) {}
  isLoading = true;
  errorMessage: string | null = null;
  incidentTypes: Array<IncidentType> = [];

  /**
   * Used for angular to run change detection when ID is not null but it's value changes
   */
  _selectedIncidentTypeIdChangeDectection: string[] = [];

  fetchData() {
    this.isLoading = true;
    this.errorMessage = null;
    this._selectedIncidentTypeIdChangeDectection = [];

    this.incidentTypeService.getAllIncidentTypes().subscribe({
      next: (value) => {
        this.incidentTypes = value;
        this.isLoading = false;
      },
      error: (err: HttpErrorResponse) => {
        this.errorMessage = err.error[0]?.code;
        this.isLoading = false;
      },
    });
  }

  ngOnInit(): void {
    this.fetchData();
  }

  /**
   * Fires off when a incident type is clicked
   */
  IncidentTypeClickListener(event: IncidentTypeClickedEvent) {
    this._selectedIncidentTypeIdChangeDectection = [];
    this._selectedIncidentTypeIdChangeDectection.push(event.id!);
  }

  /**
   * Ran when a incident type is created so we can refresh the stale data in the UI
   */
  IncidentTypeCreatedListener() {
    this.fetchData();
  }
}
