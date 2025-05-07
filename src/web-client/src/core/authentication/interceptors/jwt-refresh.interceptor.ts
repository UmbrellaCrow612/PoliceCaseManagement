import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandlerFn,
  HttpRequest,
} from '@angular/common/http';
import {
  catchError,
  Observable,
  switchMap,
  throwError,
  finalize,
  take,
  tap,
} from 'rxjs';
import { ReplaySubject } from 'rxjs';
import { StatusCodes } from '../../http/codes/status-codes'; 
import { inject } from '@angular/core';
import { AuthenticationService } from '../services/authentication.service';

// Shared state for token refreshing
let isRefreshingToken = false;
// ReplaySubject(1) will emit the last token (or error) to new subscribers.
// This subject will specifically emit the new JWT string upon successful refresh.
let refreshTokenSubject = new ReplaySubject<string>(1);

export function jwtRefreshInterceptor(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> {
  const authService = inject(AuthenticationService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {

      if (error.status === StatusCodes.UNAUTHORIZED) {
        // Prevent refresh token loop if the refresh token request itself fails.
        if (req.url.includes(authService.REFRESH_TOKEN_URL)) {
          console.error('Refresh token request itself failed with 401. Logging out.');
          isRefreshingToken = false; // Reset state
          refreshTokenSubject.error(new Error('Refresh token failed')); // Signal error to any waiters
          refreshTokenSubject = new ReplaySubject<string>(1); // Reset subject for future attempts
          authService.Logout();
          return throwError(() => error); // Propagate the original error
        }

        if (!isRefreshingToken) {
          isRefreshingToken = true;
          // Create a new subject for this specific refresh attempt.
          // This ensures that any previous state (like an error) of the subject
          // doesn't affect the current refresh attempt.
          refreshTokenSubject = new ReplaySubject<string>(1);

          return authService.refreshToken().pipe(
            switchMap((response) => {
              // Token successfully refreshed.
              // The authService.refreshToken() method already sets the token in memory via its tap operator.
              refreshTokenSubject.next(response.jwtBearerToken); // Emit the new token for waiting requests
              refreshTokenSubject.complete(); // Signal that this refresh cycle is done

              // Retry the original request that triggered the refresh
              const newRequest = req.clone({
                setHeaders: {
                  Authorization: `Bearer ${response.jwtBearerToken}`,
                },
              });
              return next(newRequest);
            }),
            catchError((refreshError) => {
              // Refresh token failed
              refreshTokenSubject.error(refreshError); // Signal failure to waiting requests
              // refreshTokenSubject.complete(); // Error also completes the subject implicitly

              console.error('Token refresh failed. Logging out.', refreshError);
              authService.Logout();
              return throwError(() => error); // Propagate the original 401 error that triggered this
            }),
            finalize(() => {
              // This block executes whether the refresh succeeds or fails.
              isRefreshingToken = false;
            })
          );
        } else {
          // A refresh is already in progress, wait for its outcome.
          return refreshTokenSubject.pipe(
            take(1), // We only care about the one result (token or error) from the ongoing refresh.
            switchMap((refreshedJwt) => {
              // Refresh was successful (refreshTokenSubject emitted a new JWT).
              // Retry THIS queued request with the new token.
              const newRequest = req.clone({
                setHeaders: {
                  Authorization: `Bearer ${refreshedJwt}`,
                },
              });
              return next(newRequest);
            }),
            catchError(() => {
              // This catchError is for the refreshTokenSubject.pipe stream.
              // It means refreshTokenSubject emitted an error (i.e., the shared refresh failed).
              // Logout is handled by the primary refresh logic that initiated the refresh.
              // We just propagate the original 401 error for this specific queued request.
              return throwError(() => error);
            })
          );
        }
      } else if (error.status === StatusCodes.FORBIDDEN) {
        authService.UnAuthorized();
        return throwError(() => error);
      }

      // For other errors, just propagate them
      return throwError(() => error);
    })
  );
}