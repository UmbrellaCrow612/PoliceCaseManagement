import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import env from '../../../environments/environment';
import { Router } from '@angular/router';
import { appPaths } from '../../app/constants/appPaths';
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  private readonly BASE_URL = env.BaseUrls.authenticationBaseUrl;
  public readonly REFRESH_TOKEN_URL = `/authentication/refresh-token`
  private JWT_TOKEN: string = '';

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
        })
      );
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
