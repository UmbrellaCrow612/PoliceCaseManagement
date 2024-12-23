import { HttpEvent, HttpHandlerFn, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import CustomHeaderNames from '../../../http/headers/constants/names';

export function DeviceFingerPrintInterceptor(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> {
  
  const reqWithHeader = req.clone({
    headers: req.headers.set(CustomHeaderNames.DEVICE_FINGERPRINT, 'new header value'),
  });

  return next(reqWithHeader);
}
