import { AbstractControl, ValidationErrors } from '@angular/forms';

export function validUrl(control: AbstractControl): ValidationErrors | null {
  try {
    new URL(control.value);
    return null;
  } catch {
    return { url: 'invalid URL', value: control.value };
  }
}
