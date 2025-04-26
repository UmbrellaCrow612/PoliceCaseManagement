import { HttpErrorResponse } from '@angular/common/http';

/**
 * Helper util to turn backend error response into a readable message
 * @param error HTTP error response
 * @returns String message formatted into a readable message
 */
export function formatBackendError(error: HttpErrorResponse) {
  let message = 'Failed request: ';

  const object = error.error;

  /** Handles obj = {errors: [{code, message}]} */
  if (typeof object === 'object') {
    if (object.errors && object.errors[0]?.code) {
      message += `code: ${object.errors[0].code} message: ${
        object.errors[0]?.message ? object.errors[0]?.message : 'No Message'
      }`;

      return message;
    }
  }

  /* Handle object = [{code:123, mess:123}] */
  if (typeof object === 'object') {
    if (Array.isArray(object) && object[0]?.code) {
      message += `code: ${object[0]?.code} message: ${
        object[0]?.message ? object[0]?.message : 'No message'
      }`;
      return message;
    }
  }

  /* Handle obj = ["string array"] */
  if (typeof object === 'object') {
    if (Array.isArray(object) && typeof object[0] === 'string') {
      message += `message: ${object[0]}`;
      return message;
    }
  }

  /* Handle validation error */
  if (typeof object === 'object') {
    if (
      object.validationErrors &&
      typeof object.validationErrors[0] === 'string'
    ) {
      message += `Valadation error: ${object.validationErrors[0]}`;
      return message;
    }
  }

  /* Handles appendix 1 */
  if (typeof object === 'object') {
    if (
      object.errors &&
      Object.keys(object.errors)[0] &&
      typeof Array.isArray(object.errors[Object.keys(object.errors)[0]]) &&
      typeof object.errors[Object.keys(object.errors)[0]][0] === 'string'
    ) {
      message += `Valadation error: ${object.errors[Object.keys(object)[0]][0]}`;
    }
  }

  return message;
}


/**
 * Appendix one
{
"type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
"title": "One or more validation errors occurred.",
"status": 400,
"errors": {
  "Status": [
    "The value '1000' is invalid."
  ]
},
"traceId": "00-bdde4b99f104b497e76cb618c15da550-122cef122f0a65cf-00"
}
 */
