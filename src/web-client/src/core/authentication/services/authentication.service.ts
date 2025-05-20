import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import env from '../../../environments/environment';
import { Router } from '@angular/router';
import { appPaths } from '../../app/constants/appPaths';
import { tap } from 'rxjs';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  private readonly BASE_URL = env.BaseUrls.authenticationBaseUrl;
  public readonly REFRESH_TOKEN_URL = `/authentication/refresh-token`;
  private JWT_TOKEN: string = '';
  private refreshTokenTimeout: any;

  constructor(private http: HttpClient, private router: Router) {}

  /**
   * Used to refresh the token and set the jwt in memeory of the app
   * called on page load first thing
   */
  refreshToken() {
    return this.http
      .get<{ jwtBearerToken: string }>(
        `${this.BASE_URL}${this.REFRESH_TOKEN_URL}`
      )
      .pipe(
        tap((response) => {
          this.setJwtBearerToken(response.jwtBearerToken);
          this.setRefreshTokenTimer();
        })
      );
  }

  private setRefreshTokenTimer() {
    if (this.JWT_TOKEN.trim() !== '') {
      const decoded: any = jwtDecode(this.JWT_TOKEN);

      const expiresAt = decoded.exp * 1000;

      const now = Date.now();
      const timeout = expiresAt - now - 60 * 1000;

      if (timeout > 0) {
        clearTimeout(this.refreshTokenTimeout);

        // Set new timer
        this.refreshTokenTimeout = setTimeout(() => {
          this.refreshToken().subscribe(); // Automatically refresh
        }, timeout);

        console.log(
          `Refresh token timer set to fire in ${timeout / 1000} seconds`
        );
      } else {
        console.warn(
          'JWT already expired or timeout too short. Refreshing now.'
        );
        this.refreshToken().subscribe();
      }
    }
  }

  getJwtBearerToken() {
    return this.JWT_TOKEN;
  }

  setJwtBearerToken(value: string) {
    this.JWT_TOKEN = value;
  }

  Login(email: string, password: string) {
    return this.http.post<{ loginAttemptId: string }>(
      `${this.BASE_URL}/authentication/login`,
      { email: email, password: password }
    );
  }

  SendTwoFactorSmsCode(loginAttemptId: string) {
    return this.http.post(
      `${this.BASE_URL}/authentication/send-two-factor-sms`,
      { loginAttemptId: loginAttemptId }
    );
  }

  ValidateTwoFactorSmsCode(loginAttemptId: string, code: string) {
    return this.http
      .post<{ jwtBearerToken: string }>(
        `${this.BASE_URL}/authentication/validate-two-factor-sms`,
        { loginAttemptId: loginAttemptId, code: code }
      )
      .pipe(
        tap((response) => {
          this.setJwtBearerToken(response.jwtBearerToken);
          this.setRefreshTokenTimer();
        })
      );
  }

  SendConfirmationEmail(email: string) {
    return this.http.post(
      `${this.BASE_URL}/authentication/send-confirmation-email`,
      { email: email }
    );
  }

  SendConfirmationEmailCode(email: string, code: string) {
    return this.http.post(`${this.BASE_URL}/authentication/confirm-email`, {
      email: email,
      code: code,
    });
  }

  SendPhoneConfirmation(phoneNumber: string) {
    return this.http.post(
      `${this.BASE_URL}/authentication/send-phone-confirmation`,
      { phoneNumber: phoneNumber }
    );
  }

  /**
   * Used to send the code sent to users device which they type into a field - this is used to confirm there phone number
   * using said code attempt
   */
  ValidatePhoneConfirmationCode(phoneNumber: string, code: string) {
    return this.http.post(
      `${this.BASE_URL}/authentication/validate-phone-confirmation`,
      { phoneNumber: phoneNumber, code: code }
    );
  }

  /**
   * Hits the backend to remove http cookies and go to login page
   */
  Logout() {
    clearTimeout(this.refreshTokenTimeout);
    this.http.get(`${this.BASE_URL}/authentication/logout`).subscribe({
      next: () => {
        this.router.navigate([appPaths.AUTHENTICATION, appPaths.LOGIN]);
      },
      error: () => {
        this.router.navigate([appPaths.AUTHENTICATION, appPaths.LOGIN]);
      },
    });
  }

  /**
   * Helper to navigate to unauthorized page
   */
  UnAuthorized() {
    this.router.navigate([appPaths.AUTHENTICATION, appPaths.UNAUTHORIZED]);
  }

  /**
   * Change a users password - provide the current password and new password
   */
  ChangePassword(
    body: Partial<{
      email: string | null;
      password: string | null;
      newPassword: string | null;
    }>
  ) {
    return this.http.post(
      `${this.BASE_URL}/authentication/change-password`,
      body
    );
  }
}
