import { HttpEvent, HttpHandlerFn, HttpRequest } from "@angular/common/http";
import { inject } from "@angular/core";
import { Observable } from "rxjs";
import { AuthenticationService } from "../services/authentication.service";

export function JwtInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {

  const authService = inject(AuthenticationService)
  var token = authService.GetJwtToken();

  const reqWithHeader = req.clone({
    headers: req.headers.set('Authorization', `Bearer ${token}`),
  });

  return next(reqWithHeader);
}