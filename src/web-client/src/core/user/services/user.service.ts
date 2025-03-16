import { Injectable } from '@angular/core';
import { FetchUserResponseBody, User } from '../type';
import { BaseService } from '../../http/services/BaseService.service';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import env from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class UserService extends BaseService {
  private BASE_URL = env.BaseUrls.authenticationBaseUrl;

  /**
   * Current user from the backend
   */
  USER: User | null = null;

  /**
   * The current users roles
   */
  ROLES: string[] | null = null;

  constructor(httpClient: HttpClient) {
    super(httpClient);
  }

  /**
   * Run at the initialization of the app to set the current user in context so it's values can be accessed across the
   * application with the USER and ROLES fields as well on successful login when we redirect to x page
   * call set user there so in the whole app it should be in two places.
   */
  setCurrentUser() {
    this.get<FetchUserResponseBody>(`${this.BASE_URL}/users/me`).subscribe({
      next: (val) => {
        (this.USER = val.user), (this.ROLES = val.roles);
      },
      error: (err: HttpErrorResponse) => {
        // failed could be not authed
        console.error('Failed to set current user in context');
      },
    });
  }
}
