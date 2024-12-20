import { HttpEvent, HttpHandlerFn, HttpRequest } from "@angular/common/http";
import { Observable } from "rxjs";

export function JwtInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {

  console.debug("JWT interceptor ran on this request.");

  const reqWithHeader = req.clone({
    headers: req.headers.set('Authorization', `Bearer `),
  });

  return next(reqWithHeader);
}