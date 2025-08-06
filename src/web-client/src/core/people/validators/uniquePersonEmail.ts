import { inject, Injectable } from '@angular/core';
import {
  AbstractControl,
  AsyncValidator,
  ValidationErrors,
} from '@angular/forms';
import { PeopleService } from '../services/people.service';
import { catchError, map, Observable, of } from 'rxjs';

/**
 * Helper validator to check is a person email adress is taken by another person in the system
 * If not unique error `uniqueEmail` on control
 */
@Injectable({ providedIn: 'root' })
export class UniquePersonEmailValidator implements AsyncValidator {
  private readonly peopleService = inject(PeopleService);

  validate(control: AbstractControl): Observable<ValidationErrors | null> {
    return this.peopleService.isEmailTaken(control.value).pipe(
      map((response) => (response.taken ? { uniqueEmail: true } : null)),
      catchError(() => of(null))
    );
  }
}
