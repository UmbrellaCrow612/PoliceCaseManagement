import { Injectable } from '@angular/core';
import { FetchUserResponseBody, User } from '../type';
import { BaseService } from '../../http/services/BaseService.service';
import { HttpClient } from '@angular/common/http';
import env from '../../../environments/environment';
import { catchError, of, tap } from 'rxjs';

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

  getCurrentUser() {
    return this.get<FetchUserResponseBody>(`${this.BASE_URL}/users/me`).pipe(
      tap({
        next: (val) => {
          this.USER = val.user;
          this.ROLES = val.roles;
        },
      })
    );
  }
}
