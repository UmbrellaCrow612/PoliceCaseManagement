import { HttpEvent, HttpHandlerFn, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { inject } from '@angular/core';
import { DeviceService } from '../services/device.service';
import CustomHeaderNames from '../../headers/constants/names';

export function DeviceFingerPrintInterceptor(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> {

  var deviceService = inject(DeviceService);

  var fingerPrint = deviceService.GetDeviceFingerPrint();

  const reqWithHeader = req.clone({
    headers: req.headers.set(CustomHeaderNames.DEVICE_FINGERPRINT, fingerPrint),
  });

  return next(reqWithHeader);
}
