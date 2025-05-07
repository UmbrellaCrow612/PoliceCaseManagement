import {
  ApplicationConfig,
  inject,
  provideAppInitializer,
  provideZoneChangeDetection,
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { deviceFingerPrintInterceptor } from '../core/user/device/interceptors/device.interceptor';
import { jwtRefreshInterceptor } from '../core/authentication/interceptors/jwt-refresh.interceptor';
import { UserService } from '../core/user/services/user.service';
import { catchError, of, switchMap } from 'rxjs';
import { withCredentialsInterceptor } from '../core/http/interceptors/withCredentails.interceptor';
import { provideMomentDateAdapter } from '@angular/material-moment-adapter';
import { AuthenticationService } from '../core/authentication/services/authentication.service';
import { jwtBearerInterceptor } from '../core/authentication/interceptors/jwt-bearer.interceptor';
import { Location } from '@angular/common';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideAnimationsAsync(),
    provideHttpClient(
      withInterceptors([
        deviceFingerPrintInterceptor,
        withCredentialsInterceptor,
        jwtRefreshInterceptor,
        jwtBearerInterceptor,
      ])
    ),
    provideAppInitializer(() => {
      const userService = inject(UserService);
      const authService = inject(AuthenticationService);
      const location = inject(Location);

      const currentPath = location.path();
      const isAuthRoute = currentPath.includes('/authentication');

      if (isAuthRoute) {
        console.log(
          'Skipping app init logic on authentication route:',
          currentPath
        );
        return of(null);
      }

      return authService.refreshToken().pipe(
        catchError((error) => {
          console.error(
            'Failed to refresh token during app initialization:',
            error
          );
          // Ensure catchError returns an Observable for the stream to continue
          return of(null);
        }),
        switchMap(() => {
          // This will be executed regardless of refreshToken success or failure (due to catchError)
          return userService.getCurrentUser().pipe(
            catchError(() => {
              console.error(
                'Failed to get current user during app initialization.'
              );
              // Ensure catchError returns an Observable
              return of(null);
            })
          );
        })
      );
    }),
    provideMomentDateAdapter(undefined, { useUtc: true }),
  ],
};
