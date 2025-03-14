import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

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
        withCredentials: true
      })
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
    return this.http.post<T>(url, body, {
      headers,
      withCredentials: true, 
    });
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
  }
}
