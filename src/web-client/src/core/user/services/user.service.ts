import { CreateUserResponseBody } from './../type';
import { Injectable } from '@angular/core';
import { FetchUserResponseBody, User } from '../type';
import { HttpClient } from '@angular/common/http';
import env from '../../../environments/environment';
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private BASE_URL = env.BaseUrls.authenticationBaseUrl;

  /**
   * Current user from the backend
   */
  USER: User | null = null;

  /**
   * The current users roles
   */
  ROLES: string[] | null = null;

  constructor(private httpClient: HttpClient) {}

  getCurrentUser() {
    return this.httpClient
      .get<FetchUserResponseBody>(`${this.BASE_URL}/users/me`)
      .pipe(
        tap({
          next: (val) => {
            this.USER = val.user;
            this.ROLES = val.roles;
          },
        })
      );
  }

  /**
   * Helper method to see if a username is taken - typically used when creating a user and need to validate that
   * the typed username is not taken
   */
  isUsernameTaken(body: { username: string }) {
    return this.httpClient.post(
      `${this.BASE_URL}/users/usernames/is-taken`,
      body
    );
  }

  /**
   * Helper method to see if a users email is taken already by another user
   */
  isEmailTaken(body: { email: string }) {
    return this.httpClient.post(`${this.BASE_URL}/users/emails/is-taken`, body);
  }

  /**
   * Helper method to see if a users phone number is taken by another
   */
  isPhoneNumberTaken(body: { phoneNumber: string }) {
    return this.httpClient.post(
      `${this.BASE_URL}/users/phone-numbers/is-taken`,
      body
    );
  }

  /**
   * Creates a user into the system - only admins can do this.
   */
  createUser(body: {
    userName: string;
    email: string;
    phoneNumber: string;
    password: string;
  }) {
    return this.httpClient.post<CreateUserResponseBody>(
      `${this.BASE_URL}/authentication/register`,
      body
    );
  }

  /**
   * Get a users detail by there ID - done by a admin.
   */
  getUserById(body: { userId: string }) {
    return this.httpClient.get<User>(`${this.BASE_URL}/users/${body.userId}`);
  }
}
