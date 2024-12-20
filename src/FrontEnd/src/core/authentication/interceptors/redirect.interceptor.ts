import {
  HttpEvent,
  HttpEventType,
  HttpHandlerFn,
  HttpRequest,
} from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { StatusCodes } from '../../codes/status-codes';

export function RedirectInterceptor(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> {
  return next(req).pipe(
    tap((event) => {
      if (event.type === HttpEventType.Response) {
        console.log(req.url, 'returned a response with status', event.status);

        if(event.status === StatusCodes.FORBIDDEN){
            console.log('Redirecting to the given url sent with the response from dotnet server');
            // Redirect to page defined
        }
      }
    })
  );
}
