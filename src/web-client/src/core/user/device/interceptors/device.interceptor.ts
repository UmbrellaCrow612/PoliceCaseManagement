import { HttpEvent, HttpHandlerFn, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import CustomHeaderNames from '../../../http/headers/constants/names';
import { inject } from '@angular/core';
import { DeviceService } from '../services/device.service';

export function DeviceFingerPrintInterceptor(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> {

  var deviceService = inject(DeviceService);

  var fingerprint = deviceService.GetDeviceFingerPrint();
  
  const reqWithHeader = req.clone({
    headers: req.headers.set(CustomHeaderNames.DEVICE_FINGERPRINT, fingerprint),
  });

  return next(reqWithHeader);
}
