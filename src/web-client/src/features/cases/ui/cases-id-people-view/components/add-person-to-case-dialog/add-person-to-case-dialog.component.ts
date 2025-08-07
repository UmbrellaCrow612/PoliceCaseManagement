import { Component, inject, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { PaginatedResult } from '../../../../../../core/app/type';
import { Person } from '../../../../../../core/people/types';
import { CommonModule } from '@angular/common';
import { MatFormField } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { PeopleService } from '../../../../../../core/people/services/people.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-add-person-to-case-dialog',
  imports: [
    MatDialogModule,
    MatButtonModule,
    CommonModule,
    ReactiveFormsModule,
    MatFormField,
    MatInputModule,
    MatDatepickerModule,
  ],
  templateUrl: './add-person-to-case-dialog.component.html',
  styleUrl: './add-person-to-case-dialog.component.css',
  providers: [provideNativeDateAdapter()],
})
export class AddPersonToCaseDialogComponent implements OnInit {
  private readonly peopleService = inject(PeopleService);

  /**
   * The ID of the case it is opened up for
   */
  private readonly caseId = inject<{ caseId: string }>(MAT_DIALOG_DATA).caseId;

  /**
   * Holds any error state of the dialog
   */
  error: string | null = null;

  /**
   * Contaisn all fields to search people
   */
  searchPeopleForm = new FormGroup({
    firstName: new FormControl<string | null>(null),
    lastName: new FormControl<string | null>(null),
    dateOfBirth: new FormControl<Date | null>(null),
  });

  /**
   * Contains the fields and state for linking a given person and there role for the case
   */
  assignPersonForm = new FormGroup({
    /**
     * Holds the selected person to link to the case
     */
    selectedPersonControl: new FormControl<Person | null>(null, {
      validators: [Validators.required],
    }),
    /**
     * Role the person has on the case
     */
    personRoleControl: new FormControl<number | null>(null, {
      validators: [Validators.required],
    }),
  });

  /**
   * All the people searched based on the search terms
   */
  searchedPeople: PaginatedResult<Person> | null = null;

  /**
   * Contains the loading state for when fetching people
   */
  isSearching = false;

  ngOnInit(): void {
    if (!this.caseId) {
      this.error = 'Failed to recieve case ID from parent';
    }
  }

  /**
   * Runs when searching for people based on criertia
   */
  searchPeople() {
    this.isSearching = true;
    this.error = null;

    this.peopleService
      .search({
        dateOfBirth: this.searchPeopleForm.controls.dateOfBirth.value,
        email: null,
        firstName: this.searchPeopleForm.controls.firstName.value,
        lastName: this.searchPeopleForm.controls.lastName.value,
        pageNumber: 1,
        pageSize: 10,
        phoneNumber: null,
      })
      .subscribe({
        next: (response) => {
          this.searchedPeople = response;
          this.isSearching = false;
        },
        error: (err: HttpErrorResponse) => {
          this.isSearching = false;
          this.error = 'Failed to search people';
        },
      });
  }
}
