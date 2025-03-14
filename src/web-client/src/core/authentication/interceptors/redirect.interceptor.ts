import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandlerFn,
  HttpRequest,
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { StatusCodes } from '../../http/codes/status-codes';
import { inject } from '@angular/core';
import { Router } from '@angular/router';

export function authRedirectsInterceptor(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> {
  var router = inject(Router);
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      console.log('RedirectsInterceptor running for error:', error.error);

      if (
        error.status === StatusCodes.UNAUTHORIZED ||
        error.status === StatusCodes.FORBIDDEN
      ) {
        
        return throwError(() => error);
      }
      return throwError(() => error);
    })
  );
}
