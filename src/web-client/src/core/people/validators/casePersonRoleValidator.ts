import { AbstractControl, ValidationErrors } from '@angular/forms';
import { CasePersonRoleNames } from '../types';

/**
 * Custom validator for validating a person's role in a case.
 *
 * This checks that:
 * - The control's value is a number.
 * - The number matches one of the valid `CasePersonRoleNames` values.
 *
 * If the control is empty (`null` or `undefined`), the validator returns `null`
 * so it can be combined with the `Validators.required` validator if the field is mandatory.
 *
 * @param control - The form control containing the role value to validate.
 * @returns
 * - `null` if the value is valid or not provided.
 * - `{ casePersonRole: true }` if the value is not a valid role.
 *
 * @errorKey `casePersonRole` — Indicates the provided role value is invalid.
 *
 * @example
 * formControl.hasError('casePersonRole') // true if invalid role
 */
export default function Validator_CasePersonRole(
  control: AbstractControl<number>
): ValidationErrors | null {
  const value = control.value;

  if (value == null) {
    return null; // No value provided, skip validation here
  }

  if (
    typeof value !== 'number' ||
    !CasePersonRoleNames.some((x) => x.value === value)
  ) {
    return { casePersonRole: true };
  }

  return null;
}
