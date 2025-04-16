import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IncidentTypeService } from '../../../../../core/incident-type/services/incident-type-service.service';
import { IncidentType } from '../../../../../core/incident-type/types';
import { CommonModule } from '@angular/common';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-system-incident-type-id-view',
  imports: [CommonModule],
  templateUrl: './system-incident-type-id-view.component.html',
  styleUrl: './system-incident-type-id-view.component.css',
})
export class SystemIncidentTypeIdViewComponent implements OnInit {
  constructor(
    private active: ActivatedRoute,
    private incidentTypeService: IncidentTypeService
  ) {}

  isLoading = true;
  errorMessage: string | null = null;
  /**
   * Id from the url for a incident type
   */
  incidentTypeId: string | null = null;

  /**
   * Details about incident type fetched from thr Db
   */
  incidentType: IncidentType | null = null;

  /**
   * How many times this incident type is linked to any number of cases
   */
  caseIncidentTypeCount = 0;

  fetchData() {
    if (!this.incidentTypeId) {
      return;
    }
    this.isLoading = true;
    this.errorMessage = null;

    forkJoin({
      incidentType: this.incidentTypeService.getIncidentTypeById(
        this.incidentTypeId
      ),
      caseIncidentTypeCount: this.incidentTypeService.getCaseIncidentTypeCount(
        this.incidentTypeId
      ),
    }).subscribe({
      next: (response) => {
        (this.incidentType = response.incidentType),
          (this.caseIncidentTypeCount = response.caseIncidentTypeCount.count);
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = 'Failed to get details';
        this.isLoading = false;
      },
    });
  }

  ngOnInit(): void {
    this.incidentTypeId = this.active.snapshot.paramMap.get('incidentTypeId');
    if (!this.incidentTypeId) {
      this.errorMessage = 'Failed to get incident type ID from URL';
      return;
    }

    this.fetchData();
  }
}
