import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class BaseService {
  constructor(protected http: HttpClient) {}

  /**
   * GET request
   * @param url The endpoint URL
   * @param params Optional query parameters
   * @param headers Optional HTTP headers
   */
  protected get<T>(
    url: string,
    params?: HttpParams | { [param: string]: string | string[] },
    headers?: HttpHeaders | { [header: string]: string | string[] }
  ): Observable<T> {
    return this.http
      .get<T>(url, {
        params: params,
        headers: headers,
      })
      .pipe(catchError(this.handleError));
  }

  /**
   * POST request
   * @param url The endpoint URL
   * @param body The request body
   * @param headers Optional HTTP headers
   */
  protected post<T>(
    url: string,
    body: any,
    headers?: HttpHeaders | { [header: string]: string | string[] }
  ): Observable<T> {
    return this.http
      .post<T>(url, body, { headers })
      .pipe(catchError(this.handleError));
  }

  /**
   * PUT request
   * @param url The endpoint URL
   * @param body The request body
   * @param headers Optional HTTP headers
   */
  protected put<T>(
    url: string,
    body: any,
    headers?: HttpHeaders | { [header: string]: string | string[] }
  ): Observable<T> {
    return this.http
      .put<T>(url, body, { headers })
      .pipe(catchError(this.handleError));
  }

  /**
   * DELETE request
   * @param url The endpoint URL
   * @param params Optional query parameters
   * @param headers Optional HTTP headers
   */
  protected delete<T>(
    url: string,
    params?: HttpParams | { [param: string]: string | string[] },
    headers?: HttpHeaders | { [header: string]: string | string[] }
  ): Observable<T> {
    return this.http
      .delete<T>(url, {
        params: params,
        headers: headers,
      })
      .pipe(catchError(this.handleError));
  }

  /**
   * Handle HTTP errors
   * @param error The error response
   */
  private handleError(error: any): Observable<never> {
    console.error('An error occurred:', error);
    return throwError(() => error);
  }
}
