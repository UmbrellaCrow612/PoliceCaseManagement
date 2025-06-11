import { CommonModule } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { CasePagedResult } from '../../../../core/cases/type';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { RestrictedUser } from '../../../../core/user/type';
import { IncidentType } from '../../../../core/incident-type/types';
import { IncidentTypeService } from '../../../../core/incident-type/services/incident-type-service.service';
import { CaseService } from '../../../../core/cases/services/case.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ErrorService } from '../../../../core/app/errors/services/error.service';
import { MatListModule } from '@angular/material/list';
import { CaseStatusSelectComponent } from '../../../../core/cases/components/case-status-select/case-status-select.component';
import { CasePrioritySelectComponent } from '../../../../core/cases/components/case-priority-select/case-priority-select.component';
import { SearchUsersSelectComponent } from '../../../../core/user/components/search-users-select/search-users-select.component';
import { SearchUsersMultiSelectComponent } from '../../../../core/user/components/search-users-multi-select/search-users-multi-select.component';
import { CaseGridListComponent } from '../../../../core/cases/components/case-grid-list/case-grid-list.component';

@Component({
  selector: 'app-search-cases-view',
  imports: [
    ReactiveFormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    CommonModule,
    MatSelectModule,
    MatAutocompleteModule,
    MatListModule,
    CaseStatusSelectComponent,
    CasePrioritySelectComponent,
    SearchUsersSelectComponent,
    SearchUsersMultiSelectComponent,
    CaseGridListComponent,
  ],
  templateUrl: './search-cases-view.component.html',
  styleUrl: './search-cases-view.component.css',
})
export class SearchCasesViewComponent implements OnInit {
  constructor(
    private incidentTypeService: IncidentTypeService,
    private caseService: CaseService,
    private errorService: ErrorService
  ) {}
  ngOnInit(): void {}

  @ViewChild('incidentTypeInput')
  incidentTypeInput: ElementRef<HTMLInputElement> | null = null;

  searchCasesFrom = new FormGroup({
    caseNumber: new FormControl<string | null>(null),
    incidentDateTime: new FormControl<Date | null>(null),
    reportedDateTime: new FormControl<Date | null>(null),
    status: new FormControl<string | null>(null),
    priority: new FormControl<string | null>(null),
    reportingOfficer: new FormControl<RestrictedUser | null>(null),
    createdByUser: new FormControl<RestrictedUser | null>(null),
    incidentType: new FormControl<string | null>(null),
    assginedUsers: new FormControl<RestrictedUser[] | null>(null),
  });

  isFetchingIncidentTypes = false;
  incidentTypes: IncidentType[] | null = null;
  filteredIncidentTypes: IncidentType[] | null = null;
  fetchIncidentTypes() {
    if (!this.incidentTypes && !this.isFetchingIncidentTypes) {
      this.isFetchingIncidentTypes = true;

      this.incidentTypeService.getAllIncidentTypes().subscribe({
        next: (incidentTypes) => {
          this.incidentTypes = incidentTypes;
          this.filteredIncidentTypes = incidentTypes;
          this.isFetchingIncidentTypes = false;
        },
        error: () => {
          this.isFetchingIncidentTypes = false;
        },
      });
    }
  }

  filterIncidentTypes() {
    if (this.incidentTypes && !this.isFetchingIncidentTypes) {
      let searchTerm = this.incidentTypeInput?.nativeElement.value;
      if (searchTerm && searchTerm.trim() !== '') {
        this.filteredIncidentTypes = this.incidentTypes.filter((x) =>
          x.name
            .toLowerCase()
            .trim()
            .includes(searchTerm.trim().toLocaleLowerCase())
        );
      } else {
        this.filteredIncidentTypes = this.incidentTypes;
      }
    }
  }

  isFetchingCases = false;
  casesPagedResult: CasePagedResult | null = null;
  onSubmit() {
    this.fetchCases();
  }

  fetchCases(options: Partial<{ pageNumber: number | null }> = {}) {
    let incidentTypeId = this.incidentTypes?.find(
      (x) => x.name === this.searchCasesFrom.controls.incidentType.getRawValue()
    )?.id;

    this.isFetchingCases = true;
    this.caseService
      .searchCasesWithPagination({
        caseNumber: this.searchCasesFrom.controls.caseNumber.getRawValue(),
        incidentDateTime:
          this.searchCasesFrom.controls.incidentDateTime.getRawValue(),
        incidentTypeId: incidentTypeId,
        priority: this.searchCasesFrom.controls.priority.getRawValue(),
        reportedDateTime:
          this.searchCasesFrom.controls.reportedDateTime.getRawValue(),
        reportingOfficerId:
          this.searchCasesFrom.controls.reportingOfficer.getRawValue()?.id,
        status: this.searchCasesFrom.controls.status.getRawValue(),
        pageNumber: options.pageNumber,
        createdById:
          this.searchCasesFrom.controls.createdByUser.getRawValue()?.id,
        assignedUserIds: this.searchCasesFrom.controls.assginedUsers.value?.map(
          (x) => x.id
        ),
      })
      .subscribe({
        next: (result) => {
          this.casesPagedResult = result;
          this.isFetchingCases = false;
        },
        error: (err: HttpErrorResponse) => {
          this.isFetchingCases = false;
          this.errorService.HandleDisplay(err);
        },
      });
  }

  nextPageClicked() {
    this.fetchCases({
      pageNumber: this.casesPagedResult?.hasNextPage
        ? this.casesPagedResult.pageNumber + 1
        : null,
    });
  }

  previousPageClicked() {
    this.fetchCases({
      pageNumber: this.casesPagedResult?.hasPreviousPage
        ? this.casesPagedResult.pageNumber - 1
        : null,
    });
  }

  clearClicked() {
    this.searchCasesFrom.reset();
    this.casesPagedResult = null;
  }
}
