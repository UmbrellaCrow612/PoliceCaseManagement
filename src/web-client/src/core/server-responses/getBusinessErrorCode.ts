import { HttpErrorResponse } from '@angular/common/http';

/**
 * The string value of the result pattern error code sent across from the backend, for example the backend sometimes sends
 * error.error as [{code:"FAILED", message: "Message"}] this wil get that code string value out for you to use and
 * handle all ways the backend can send it, if it cannot understand it or the response dose not conform to the shape
 * of the result pattern it will return a empty string
 * @param error HTTP error response
 * @returns String code or a empty string if it could not find it.
 */
export function getBusinessErrorCode(error: HttpErrorResponse): string {
  const object = error.error;

  /* Handle when obj = {errors:[{code, mess}]} */
  if (typeof object === 'object') {
    if (object.errors && object.errors[0]?.code) {
      return object.errors[0].code as string;
    }
  }

  /* Handles when obj = [{code,mess}] */
  if (typeof object === 'object') {
    if (Array.isArray(object) && object[0]?.code) {
      return object[0].code as string;
    }
  }

  return '';
}
