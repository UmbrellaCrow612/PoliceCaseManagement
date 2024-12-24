import { HttpErrorResponse, HttpEvent, HttpEventType, HttpHandlerFn, HttpRequest } from "@angular/common/http";
import { catchError, Observable, tap, throwError } from "rxjs";
import { StatusCodes } from "../../http/codes/status-codes";

export function authRedirectsInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {

      if (error.status === StatusCodes.UNAUTHORIZED) {
        console.log(`Redirecting to lo9gin page`);
        return throwError(() => error);
      }

      if (error.status === StatusCodes.FORBIDDEN) {
        console.log(`Redirecting to ${error.error.redirectUrl}`);
        return throwError(() => error);
      }

      return throwError(() => error);
    })
  );
}