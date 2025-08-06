// File contains custom validators for reactive form controls none business specific

import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

/**
 * Custom validator impletion for reactive forms
 * return the key value error check for { numeric: true }
 */
export function Validator_containsOnlyNumeric(
  control: AbstractControl
): ValidationErrors | null {
  const value = control.value;
  if (typeof value === 'string') {
    value as string;

    if (value.trim() === '') {
      return null;
    }
    let regex = /^\d+$/;
    let isOnlyNumeric = regex.test(value);

    if (isOnlyNumeric) {
      return null;
    } else {
      return { numeric: true };
    }
  }
  return null;
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

/**
 * Validator function to check if the size of a file exceeds a specified maximum size.
 *
 * @param maxFileSizeBytes - The **maximum allowed file size** in **bytes**.
 * @returns A validator function that:
 * - Checks the file size.
 * - Returns a validation error object if the file size exceeds the limit.
 * - Returns `null` if the file size is within the limit.
 *
 * **Validation error object shape:**
 * ```ts
 * {
 *   maxFileSize: {
 *     requiredMaxSize: number; // max allowed size in bytes
 *     actualSize: number;      // actual file size in bytes
 *   }
 * }
 * ```
 *
 * If `maxFileSizeBytes` is less than or equal to zero, the validator **skips validation**.
 *
 * **Example usage:**
 * ```ts
 * control.setValidators(Validator_maxFileSize(1048576)); // 1 MB max size
 * ```
 */
export function Validator_maxFileSize(maxFileSizeBytes: number): ValidatorFn {
  return (control: AbstractControl<File | null>): ValidationErrors | null => {
    const file = control.value;

    if (!file || !(file instanceof File) || maxFileSizeBytes <= 0) {
      return null;
    }

    const size = file.size;

    return size > maxFileSizeBytes
      ? { maxFileSize: { requiredMaxSize: maxFileSizeBytes, actualSize: size } }
      : null;
  };
}


/**
 * Validator for international phone number format.
 * Ensures the phone number:
 * - Starts with a `+`
 * - Is followed by digits only
 * - Has a reasonable length (8+ digits including country code)
 *
 * Returns:
 * - null if valid
 * - { phone: true } if invalid
 */
export function Validator_phoneNumber(control: AbstractControl): ValidationErrors | null {
  const value = control.value;

  if (typeof value !== 'string' || value.trim() === '') {
    return null; // Let required validator handle empty if needed
  }

  // E.164 basic pattern: starts with + followed by 8 to 15 digits
  const regex = /^\+\d{8,15}$/;

  return regex.test(value) ? null : { phone: true };
}
