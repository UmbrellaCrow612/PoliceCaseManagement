import { Component, inject } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { PeopleService } from '../../../../core/people/services/people.service';
import { HttpErrorResponse } from '@angular/common/http';
import { PaginatedResult } from '../../../../core/app/type';
import { Person } from '../../../../core/people/types';
import { Validator_containsOnlyNumeric } from '../../../../core/app/validators/controls';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { CreatePersonDialogComponent } from './components/create-person-dialog/create-person-dialog.component';

@Component({
  selector: 'app-people-home-view',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatButtonModule,
  ],
  templateUrl: './people-home-view.component.html',
  styleUrl: './people-home-view.component.css',
  providers: [provideNativeDateAdapter()],
})
export class PeopleHomeViewComponent {
  private readonly peopleService = inject(PeopleService);
  private readonly dialog = inject(MatDialog);

  /**
   * Contains all the fields a person can  be searched with in the system
   */
  searchPeopleForm = new FormGroup({
    firstName: new FormControl<string | null>(null),
    lastName: new FormControl<string | null>(null),
    dateOfBirth: new FormControl<Date | null>(null),
    phoneNumber: new FormControl<string | null>(null, [
      Validator_containsOnlyNumeric,
    ]),
    email: new FormControl<string | null>(null, [Validators.email]),
  });

  /**
   * Search result of matching people from the backend
   */
  searchedPeople: PaginatedResult<Person> | null = null;

  /**
   * Holds loading state for when fetching people for search
   */
  isSearching = false;

  /**
   * Holds error state for when fetchiong people for search
   */
  searchError: string | null = null;

  /**
   * Runs when search is clicked
   */
  handleSearchClicked(pageNumber: number = 1) {
    if (this.searchPeopleForm.invalid) {
      this.searchError = 'Invalid form state';
      return;
    }

    this.isSearching = true;
    this.searchError = null;

    this.peopleService
      .search({
        dateOfBirth: this.searchPeopleForm.controls.dateOfBirth.value,
        email: this.searchPeopleForm.controls.email.value,
        firstName: this.searchPeopleForm.controls.firstName.value,
        lastName: this.searchPeopleForm.controls.lastName.value,
        phoneNumber: this.searchPeopleForm.controls.phoneNumber.value,
        pageNumber: pageNumber,
        pageSize: 10,
      })
      .subscribe({
        next: (response) => {
          this.searchedPeople = response;
          this.isSearching = false;
        },
        error: (err: HttpErrorResponse) => {
          this.searchError = 'Failed to search';
          this.isSearching = false;
        },
      });
  }

  /**
   * Runs when add person is clicked
   */
  handleAddClicked() {
    this.dialog.open(CreatePersonDialogComponent);
  }
}
