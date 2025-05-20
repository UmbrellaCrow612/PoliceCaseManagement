import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandlerFn,
  HttpRequest,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { AuthenticationService } from '../services/authentication.service';
import { StatusCodes } from '../../http/codes/status-codes';

/**
 * Will run on every request and if we get a 401 status code it will logout the user
 */
export function unauthorizedInterceptor(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> {
  const authService = inject(AuthenticationService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === StatusCodes.UNAUTHORIZED) {
        authService.Logout();
      }
      return throwError(() => error);
    })
  );
}
