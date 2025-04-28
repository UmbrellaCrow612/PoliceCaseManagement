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
import { DeviceFingerPrintInterceptor } from '../core/user/device/interceptors/device.interceptor';
import { authRedirectsInterceptor } from '../core/authentication/interceptors/redirect.interceptor';
import { UserService } from '../core/user/services/user.service';
import { catchError, firstValueFrom, of } from 'rxjs';
import { withCredentialsInterceptor } from '../core/http/interceptors/withCredentails.interceptor';
import { provideMomentDateAdapter } from '@angular/material-moment-adapter';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideAnimationsAsync(),
    provideHttpClient(
      withInterceptors([
        DeviceFingerPrintInterceptor,
        authRedirectsInterceptor,
        withCredentialsInterceptor,
      ])
    ),
    provideAppInitializer(() => {
      const userService = inject(UserService);
      return firstValueFrom(
        userService.getCurrentUser().pipe(
          catchError(() => {
            return of(null);
          })
        )
      );
    }),
    provideMomentDateAdapter(undefined, { useUtc: true }),
  ],
};
