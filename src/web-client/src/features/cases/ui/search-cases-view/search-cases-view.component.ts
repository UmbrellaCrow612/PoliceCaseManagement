import { CommonModule } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import {
  CasePagedResult,
  CasePriorityNames,
  CaseStatusNames,
} from '../../../../core/cases/type';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { RestrictedUser } from '../../../../core/user/type';
import { UserService } from '../../../../core/user/services/user.service';
import { debounceTime, Subject, timer } from 'rxjs';
import { IncidentType } from '../../../../core/incident-type/types';
import { IncidentTypeService } from '../../../../core/incident-type/services/incident-type-service.service';
import { CaseService } from '../../../../core/cases/services/case.service';
import { tryConvertStringToNumber } from '../../../../core/app/utils/convert-string-to-number';
import { HttpErrorResponse } from '@angular/common/http';
import { ErrorService } from '../../../../core/app/errors/services/error.service';
import { MatListModule } from '@angular/material/list';
import { RouterLink } from '@angular/router';
import { CaseStatusPipe } from '../../../../core/cases/pipes/caseStatusPipe';
import { CasePriorityPipe } from '../../../../core/cases/pipes/casePriorityPipe';

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
    RouterLink,
    CaseStatusPipe,
    CasePriorityPipe,
  ],
  templateUrl: './search-cases-view.component.html',
  styleUrl: './search-cases-view.component.css',
})
export class SearchCasesViewComponent implements OnInit {
  constructor(
    private userService: UserService,
    private incidentTypeService: IncidentTypeService,
    private caseService: CaseService,
    private errorService: ErrorService
  ) {}
  ngOnInit(): void {
    this.searchSubject.pipe(debounceTime(1000)).subscribe(() => {
      let searchTerm = this.input?.nativeElement.value;
      if (searchTerm && searchTerm?.trim() !== '') {
        this.userService.searchUsersByUsername(searchTerm).subscribe({
          next: (users) => {
            this.users = users;
            this.isFetchingUsers = false;
          },
          error: () => {
            this.isFetchingUsers = false;
          },
        });
      } else {
        this.isFetchingUsers = false;
        this.users = [];
      }
    });
  }

  @ViewChild('reportingOfficerInput')
  input: ElementRef<HTMLInputElement> | null = null;

  @ViewChild('incidentTypeInput')
  incidentTypeInput: ElementRef<HTMLInputElement> | null = null;

  @ViewChild('paginationButtons')
  paginationButtonsContainer: ElementRef<HTMLDivElement> | null = null;

  searchCasesFrom = new FormGroup({
    caseNumber: new FormControl<string | null>(null),
    incidentDateTime: new FormControl<Date | null>(null),
    reportedDateTime: new FormControl<Date | null>(null),
    status: new FormControl<number | null>(null),
    priority: new FormControl<number | null>(null),
    reportingOfficerUserName: new FormControl<string | null>(null),
    incidentType: new FormControl<string | null>(null),
  });

  caseStatus = CaseStatusNames;
  casePrioritys = CasePriorityNames;

  isFetchingUsers = false;
  users: RestrictedUser[] = [];
  searchSubject = new Subject<void>();
  filterUsers() {
    this.isFetchingUsers = true;
    this.searchSubject.next();
  }

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
    this.fetchCases({});
  }

  fetchCases(options: Partial<{ pageNumber: number | null }>) {
    let incidentTypeId = this.incidentTypes?.find(
      (x) => x.name === this.searchCasesFrom.controls.incidentType.getRawValue()
    )?.id;

    let priority = tryConvertStringToNumber(
      this.searchCasesFrom.controls.priority.getRawValue() as any
    ) as number;

    let reportingOfficerId = this.users.find(
      (x) =>
        x.userName ===
        this.searchCasesFrom.controls.reportingOfficerUserName.getRawValue()
    )?.id;

    let status = tryConvertStringToNumber(
      this.searchCasesFrom.controls.status.getRawValue() as any
    ) as number;

    this.isFetchingCases = true;
    this.caseService
      .searchCasesWithPagination({
        caseNumber: this.searchCasesFrom.controls.caseNumber.getRawValue(),
        incidentDateTime:
          this.searchCasesFrom.controls.incidentDateTime.getRawValue(),
        incidentTypeId: incidentTypeId,
        priority: priority,
        reportedDateTime:
          this.searchCasesFrom.controls.reportedDateTime.getRawValue(),
        reportingOfficerId: reportingOfficerId,
        status,
        pageNumber: options.pageNumber,
      })
      .subscribe({
        next: (result) => {
          this.casesPagedResult = result;
          this.isFetchingCases = false;
          this.scrollToButtons();
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

  scrollToButtons() {
    timer(100).subscribe(() => {
      this.paginationButtonsContainer?.nativeElement.scrollIntoView({
        behavior: 'smooth',
        block: 'start',
      });
    });
  }

  clearClicked() {
    this.searchCasesFrom.reset();
    this.casesPagedResult = null;
  }
}
