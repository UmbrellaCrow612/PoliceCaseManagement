import { inject, Injectable } from '@angular/core';
import {
  AbstractControl,
  AsyncValidator,
  ValidationErrors,
} from '@angular/forms';
import { PeopleService } from '../services/people.service';
import { catchError, map, Observable, of } from 'rxjs';


/**
 * Helper validator to check is a person phone number is taken by another
 * Error `uniquePhoneNumber` is set if taken or null if not
 */
@Injectable({ providedIn: 'root' })
export class UniquePersonPhoneNumberValidator implements AsyncValidator {
  private readonly peopleService = inject(PeopleService);

  validate(control: AbstractControl): Observable<ValidationErrors | null> {
    return this.peopleService.isPhoneNumberTaken(control.value).pipe(
      map((response) => (response.taken ? { uniquePhoneNumber: true } : null)),
      catchError(() => of(null))
    );
  }
}
