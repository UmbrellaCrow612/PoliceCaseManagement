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
import CODES from '../../server-responses/codes';
import { appPaths } from '../../app/constants/appPaths';

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
        const code = error.error[0]?.code;

        if (typeof code === 'string') {
          console.log(`Redirecting for code ${code}`);

          var url = '';
          switch (code) {
            case CODES.EmailNotConfirmed:
              url = `${appPaths.AUTHENTICATION}/${appPaths.CONFIRM_EMAIL}`;
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
