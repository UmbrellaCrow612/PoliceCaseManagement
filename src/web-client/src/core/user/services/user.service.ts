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
   * Used to set the user in the ctx to access it values and or to see if the current user is authenticated if the user and or roles exists
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

  getCurrentUser() {
    return this.get<FetchUserResponseBody>(`${this.BASE_URL}/users/me`);
  }
}
