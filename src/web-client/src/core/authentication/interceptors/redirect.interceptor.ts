import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandlerFn,
  HttpRequest,
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { StatusCodes } from '../../http/codes/status-codes';
import { inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';

export function authRedirectsInterceptor(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> {
  let authS = inject(AuthenticationService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      console.log('RedirectsInterceptor running for error:', error.error);

      if (error.status === StatusCodes.UNAUTHORIZED) {
        // handle other like 403
        authS.Logout();
        return throwError(() => error);
      }
      return throwError(() => error);
    })
  );
}
