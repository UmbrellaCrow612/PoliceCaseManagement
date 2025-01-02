export default function formatBackendError(err: any): string {
  let errorMessage = `Status: ${err.status || 'Unknown'}`;

  // Check if the error has a specific error structure with field and reason
  if (err.error && Array.isArray(err.error) && err.error.length > 0) {
    const backendErrors = err.error;
    if (backendErrors[0].field && backendErrors[0].reason) {
      backendErrors.forEach((element: any) => {
        errorMessage += `\nField: ${element.field}\nReason: ${element.reason}`;
      });
      return errorMessage;
    }
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