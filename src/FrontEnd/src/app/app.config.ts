import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { DeviceFingerPrintInterceptor } from '../core/user/device/interceptors/device.interceptor';
import { JwtInterceptor } from '../core/authentication/interceptors/jwt.interceptor';
import { authRedirectsInterceptor } from '../core/authentication/interceptors/redirect.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideAnimationsAsync(),
    provideHttpClient(
      withInterceptors([DeviceFingerPrintInterceptor, JwtInterceptor, authRedirectsInterceptor]),
    )
  ],
};
