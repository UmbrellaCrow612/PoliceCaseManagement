import { HttpErrorResponse } from '@angular/common/http';

/**
 * Formats a backend error - be it a validation or a coded responses to a nice formatted error message string
 * @param error Http error response from the backend
 */
export function formatBackendErrorToAMessage(
  error: HttpErrorResponse // handle error code response - data validation - validator and other formats to return a nice string message etc
): string {
  return '';
}
