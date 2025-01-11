export default function formatBackendError(err: any): string {
  let errorMessage = `Status: ${err.status || 'Unknown'}`;

  if (
    err.error &&
    typeof Array.isArray(err.error) &&
    typeof err.error[0] == 'string'
  ) {
    const errs: [string] = err.error;
    errs.forEach((element) => {
      errorMessage += ` ${element} `
    });
  }

  // Check if the error has a specific error structure with field and reason
  if (
    err.error &&
    Array.isArray(err.error) &&
    err.error.length > 0 &&
    'field' in err.error &&
    'reason' in err.error
  ) {
    const backendErrors = err.error;

    backendErrors.forEach((element: any) => {
      errorMessage += `\nField: ${element.field}\nReason: ${element.reason}`;
    });

    return errorMessage;
  }

  if (
    err.error &&
    typeof err.error === 'object' &&
    'field' in err.error &&
    'reason' in err.error
  ) {
    errorMessage += `\nField: ${err.error.field}\nReason: ${err.error.reason}`;
    return errorMessage;
  }

  // Check if the error has a message property
  if (err.error && err.error.message) {
    errorMessage += `\nMessage: ${err.error.message}`;
    return errorMessage;
  }

  // Handle case where the error response is a plain string
  if (typeof err.error === 'string') {
    errorMessage += `\nMessage: ${err.error}`;
    return errorMessage;
  }

  return errorMessage;
}
