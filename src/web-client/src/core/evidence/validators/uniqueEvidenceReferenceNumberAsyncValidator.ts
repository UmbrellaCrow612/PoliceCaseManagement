import { inject, Injectable } from '@angular/core';
import {
  AbstractControl,
  AsyncValidator,
  ValidationErrors,
} from '@angular/forms';
import { catchError, map, Observable, of } from 'rxjs';
import { EvidenceService } from '../services/evidence.service';

/**
 * Custom validator to use in a input field to check if a reference number is taken in a reactive form field input check for error `uniqueReferenceNumber` with `hasError`
 * to see if it has or has not and display errors
 */
@Injectable({ providedIn: 'root' })
export class UniqueEvidenceReferenceNumberAsyncValidator
  implements AsyncValidator
{
  private readonly evidenceService = inject(EvidenceService);

  validate(control: AbstractControl): Observable<ValidationErrors | null> {
    return this.evidenceService.isReferenceNumberTaken(control.value).pipe(
      map((response) => (response.isTaken ? { uniqueReferenceNumber: true } : null)),
      catchError(() => of(null))
    );
  }
}
