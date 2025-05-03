import { Component, OnInit } from '@angular/core';
import { BackNavigationButtonComponent } from '../../../../core/components/back-navigation-button/back-navigation-button.component';
import { CaseService } from '../../../../core/cases/services/case.service';
import { IncidentTypeService } from '../../../../core/incident-type/services/incident-type-service.service';
import { forkJoin } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { ErrorService } from '../../../../core/app/errors/services/error.service';
import { IncidentType } from '../../../../core/incident-type/types';
import { CommonModule } from '@angular/common';
import { MatListModule, MatSelectionListChange } from '@angular/material/list';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-cases-id-edit-incident-type-view',
  imports: [
    BackNavigationButtonComponent,
    CommonModule,
    MatListModule,
    MatInputModule,
    MatFormFieldModule,
    MatButtonModule,
    ReactiveFormsModule,
  ],
  templateUrl: './cases-id-edit-incident-type-view.component.html',
  styleUrl: './cases-id-edit-incident-type-view.component.css',
})
export class CasesIdEditIncidentTypeViewComponent implements OnInit {
  constructor(
    private caseService: CaseService,
    private incidentTypeService: IncidentTypeService,
    private active: ActivatedRoute,
    private router: Router,
    private errorService: ErrorService
  ) {}
  ngOnInit(): void {
    this.caseId = this.active.snapshot.paramMap.get('caseId');
    if (!this.caseId) {
      this.error = 'No case id in URL';
      return;
    }
    this.fetchData();

    this.filterInput.valueChanges.subscribe({
      next: () => {
        let searchTerm = this.filterInput.value;
        if (searchTerm && searchTerm.trim() !== '') {
          this.filteredAllIncidentTypes = this.allIncidentTypes.filter((x) =>
            x.name
              .trim()
              .toLowerCase()
              .includes(searchTerm.trim().toLowerCase())
          );
        } else {
          this.filteredAllIncidentTypes = this.allIncidentTypes;
        }
      },
    });
  }

  filterInput = new FormControl<string | null>(null);

  allIncidentTypes: IncidentType[] = [];
  filteredAllIncidentTypes: IncidentType[] = [];
  currentIncidentTypes: IncidentType[] = [];
  selectedIncidentTypes = new Set<IncidentType>();

  hasUnsavedChanges = false;
  isLoading = false;
  caseId: string | null = null;
  error: string | null = null;

  fetchData() {
    if (!this.caseId) {
      return;
    }
    this.hasUnsavedChanges = false;
    this.isLoading = true;
    this.error = null;

    forkJoin({
      current: this.caseService.getIncidentTypes(this.caseId),
      all: this.incidentTypeService.getAllIncidentTypes(),
    }).subscribe({
      next: (response) => {
        (this.currentIncidentTypes = response.current),
          (this.allIncidentTypes = response.all);
        this.filteredAllIncidentTypes = response.all;
        this.selectedIncidentTypes = new Set<IncidentType>(this.currentIncidentTypes);
        this.isLoading = false;
      },
      error: (error) => {
        this.isLoading = false;
        this.errorService.HandleDisplay(error);
      },
    });
  }

  isSelected(incidentType: IncidentType): boolean {
    return this.currentIncidentTypes.find((x) => x.id == incidentType.id)
      ? true
      : false;
  }

  selectionChanged(event: MatSelectionListChange) {
    const listSelectedIncidentTypes = event.source.selectedOptions.selected.map(
      (option) => option.value
    ) as IncidentType[];
    this.selectedIncidentTypes = new Set<IncidentType>(listSelectedIncidentTypes)
  }
}
