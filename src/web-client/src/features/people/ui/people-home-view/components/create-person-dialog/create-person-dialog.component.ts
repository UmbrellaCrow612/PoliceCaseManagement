import { Component, inject } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { Validator_phoneNumber } from '../../../../../../core/app/validators/controls';
import { PeopleService } from '../../../../../../core/people/services/people.service';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { provideNativeDateAdapter } from '@angular/material/core';
import { CommonModule } from '@angular/common';
import { UniquePersonPhoneNumberValidator } from '../../../../../../core/people/validators/uniquePersonPhoneNumber';
import { UniquePersonEmailValidator } from '../../../../../../core/people/validators/uniquePersonEmail';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-create-person-dialog',
  imports: [
    MatDialogModule,
    MatButtonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    CommonModule,
  ],
  templateUrl: './create-person-dialog.component.html',
  styleUrl: './create-person-dialog.component.css',
  providers: [provideNativeDateAdapter()],
})
export class CreatePersonDialogComponent {
  private readonly peopleService = inject(PeopleService);
  private readonly ref = inject(MatDialogRef<CreatePersonDialogComponent>);
  uniquePersonPhoneNumberValidator = inject(UniquePersonPhoneNumberValidator);
  UniquePersonEmailValidator = inject(UniquePersonEmailValidator);

  /**
   * Contains all the fields to create a person and basic validation
   */
  createPersonForm = new FormGroup({
    firstName: new FormControl<string>('', [Validators.required]),
    lastName: new FormControl<string>('', [Validators.required]),
    dateOfBirth: new FormControl<Date>(new Date(), [Validators.required]),
    phoneNumber: new FormControl('', {
      validators: [Validators.required, Validator_phoneNumber],
      asyncValidators: [
        this.uniquePersonPhoneNumberValidator.validate.bind(
          this.uniquePersonPhoneNumberValidator
        ),
      ],
      updateOn: 'blur',
    }),
    email: new FormControl('', {
      validators: [Validators.required, Validators.email],
      asyncValidators: [
        this.UniquePersonEmailValidator.validate.bind(
          this.UniquePersonEmailValidator
        ),
      ],
      updateOn: 'blur',
    }),
  });

  /**
   * Holds error state
   */
  error: string | null = null;

  /**
   * Holds loading creating state
   */
  isCreating = false;

  /**
   * Runs when the form is submitted
   */
  handleSubmitForm() {
    if (this.createPersonForm.invalid) {
      this.error = 'Invalid form state';
      return;
    }

    this.error = null;
    this.isCreating = true;

    this.peopleService
      .create({
        firstName: this.createPersonForm.controls.firstName.value!,
        dateOfBirth: this.createPersonForm.controls.dateOfBirth.value!,
        email: this.createPersonForm.controls.email.value!,
        lastName: this.createPersonForm.controls.lastName.value!,
        phoneNumber: this.createPersonForm.controls.phoneNumber.value!,
      })
      .subscribe({
        next: () => {
          this.ref.close();
        },
        error: (err: HttpErrorResponse) => {
          this.error = 'Failed ot create person';
          this.isCreating = false;
          // handle err codes
        },
      });
  }
}
