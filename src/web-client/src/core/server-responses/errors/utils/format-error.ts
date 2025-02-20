import { HttpErrorResponse } from '@angular/common/http';

interface StatusCodeAndMessage {
  code: string;
  message: string;
}

export default function formatBackendError(err: HttpErrorResponse): string {
  let errorMessage = `Status: ${err.status || 'Unknown'}`;

  if (isArrayOfStatusCodeAndMessage(err.error)) {
    const errors = err.error as StatusCodeAndMessage[];
    const formattedErrors = errors
      .map((e) => `[${e.code}] ${e.message || 'No message'}`)
      .join(' | ');
    return `${errorMessage} - ${formattedErrors}`;
  }

  errorMessage += ' Message: Unknown backend error response format.';
  return errorMessage;
}

/**
 * This checks when the backend sends an error response in the shape of:
 * [
 *   {
 *     code: "CODE",
 *     message: "string or null"
 *   }
 * ]
 * @returns bool - if it is the shape or not
 */
function isArrayOfStatusCodeAndMessage(
  err: any
): err is StatusCodeAndMessage[] {
  return (
    Array.isArray(err) &&
    err.every(
      (item) => typeof item === 'object' && 'code' in item && 'message' in item
    )
  );
}
