import { HttpEvent, HttpHandlerFn, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable } from 'rxjs';
import { JwtService } from '../services/jwt.service';

export function JwtInterceptor(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> {
  var jwtService = inject(JwtService);

  /**
   * TODO add append raw token value from cookie
   */
  const newReq = req.clone({
    headers: req.headers.append('Authorization', `Bearer `),
  });

  return next(newReq);
}
