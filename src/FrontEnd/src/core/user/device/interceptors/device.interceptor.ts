import { HttpEvent, HttpHandlerFn, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { inject } from '@angular/core';
import { DeviceService } from '../services/device.service';
import CustomHeaderNames from '../../../http/headers/constants/names';

export function DeviceFingerPrintInterceptor(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> {

  return next(req);
}
