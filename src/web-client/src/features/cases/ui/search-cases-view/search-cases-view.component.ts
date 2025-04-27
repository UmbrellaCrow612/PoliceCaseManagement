import { CommonModule } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import {
  CasePriorityNames,
  CaseStatusNames,
} from '../../../../core/cases/type';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { RestrictedUser } from '../../../../core/user/type';
import { UserService } from '../../../../core/user/services/user.service';
import { debounceTime, Subject } from 'rxjs';
import { IncidentType } from '../../../../core/incident-type/types';
import { IncidentTypeService } from '../../../../core/incident-type/services/incident-type-service.service';

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
  ],
  providers: [provideNativeDateAdapter()],
  templateUrl: './search-cases-view.component.html',
  styleUrl: './search-cases-view.component.css',
})
export class SearchCasesViewComponent implements OnInit {
  constructor(
    private userService: UserService,
    private incidentTypeService: IncidentTypeService
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

  onSubmit() {}
}
