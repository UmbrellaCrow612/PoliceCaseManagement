// File contains custom validators for reactive form controls

import { AbstractControl, ValidationErrors } from '@angular/forms';

/**
 * Custom validator impletion for reactive forms
 * return the key value error check for { numeric: true }
 */
export function Validator_containsOnlyNumeric(
  control: AbstractControl
): ValidationErrors | null {
  if (typeof control.value === 'string') {
    let regex = /^\d+$/;
    const _value = control.value as string;
    let isOnlyNumeric = regex.test(_value);

    if (isOnlyNumeric) {
      return null;
    } else {
      return { numeric: true };
    }
  }
  return { numeric: true };
}

/**
 * Validator for good password strength
 * returns the key value error { password: true }
 */
export function Validator_password(
  control: AbstractControl
): ValidationErrors | null {
  if (typeof control.value === 'string') {
    let regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$/;
    let _value = control.value as string;
    let isStrongPassword = regex.test(_value);
    if (isStrongPassword) {
      return null;
    } else {
      return { password: true };
    }
  }
  return { password: true };
}
