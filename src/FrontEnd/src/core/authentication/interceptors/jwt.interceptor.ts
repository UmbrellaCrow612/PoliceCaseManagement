import { HttpEvent, HttpHandlerFn, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable } from 'rxjs';
import { JwtService } from '../services/jwt.service';

export function JwtInterceptor(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> {
  var jwtService = inject(JwtService);

  var token = jwtService.getRawJwtToken();


  // Possible idea , but could have a background task that checks token every 5 minutes or something and then make a extra request out
  // to the refresh endpoint and change the tokens in store. This would only happened or is kicked off once we make a check and 
  // see that there is already a jwt token and refresh tokens in the cookies - on logout we would need to remove the cookies themselves 
  // if (token !== null) {
  //   var isValid = jwtService.IsJwtTokenValid(token);

  //   if (!isValid) {
  //     var refreshToken = jwtService.getRawRefreshToken();

  //     if (refreshToken !== null) {
  //       jwtService.refreshToken(refreshToken);
  //     }
  //   }
  // }

  const newReq = req.clone({
    headers: req.headers.append('Authorization', `Bearer ${token}`),
  });

  return next(newReq);
}
