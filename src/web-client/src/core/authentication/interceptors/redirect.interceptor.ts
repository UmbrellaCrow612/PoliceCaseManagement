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
import REASONS from '../../server-responses/response';

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
        const reason = error.error[0]?.reason;

        if (typeof reason === 'string') {
          console.log(`Redirecting for reason ${reason}`);

          var url = '';
          switch (reason) {
            case REASONS.EmailNotConfirmed:
              url = `authentication/confirm-email`;
              break;
            default:
              break;
          }

          console.log('URL ' + url);
          if (url) {
            console.log('Navigating to  ' + url);
            router.navigate([url]);
          } else {
            console.error(url);
          }
        } else {
          console.error('Invalid response object for re-directing:', error);
        }
        return throwError(() => error);
      }
      return throwError(() => error);
    })
  );
}
