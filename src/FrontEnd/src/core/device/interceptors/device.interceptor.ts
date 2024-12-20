import { HttpEvent, HttpHandlerFn, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GetFingerPrint } from '../../../utils/device/FingerPrint';

export function DeviceFingerPrintInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {

  console.debug("Device fingerprint interceptor ran on this request.");

  var fingerPrint = GetFingerPrint();

  const reqWithHeader = req.clone({
    headers: req.headers.set('X-Device-Fingerprint', fingerPrint),
  });

  return next(reqWithHeader);
}
