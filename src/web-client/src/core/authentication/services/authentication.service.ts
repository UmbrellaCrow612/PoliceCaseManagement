import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {
  LoginResponse,
  LoginCredentials,
  SendEmailConfirmationRequest,
  SendEmailConfirmationCodeRequest,
  SendPhoneConfirmationRequest,
  ValidatePhoneConfirmationCodeRequestBody,
  SendTwoFactorSmsCodeRequestBody,
  ValidateTwoFactorSmsCodeRequestBody,
} from '../types';
import env from '../../../environments/environment';
import { Observable, timer } from 'rxjs';
import { Router } from '@angular/router';
import { appPaths } from '../../app/constants/appPaths';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  private readonly BASE_URL = env.BaseUrls.authenticationBaseUrl;

  constructor(private http: HttpClient, private router: Router) {}

  Login(credentials: LoginCredentials): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(
      `${this.BASE_URL}/authentication/login`,
      credentials
    );
  }

  SendTwoFactorSmsCode(body: SendTwoFactorSmsCodeRequestBody) {
    return this.http.post(
      `${this.BASE_URL}/authentication/send-two-factor-sms`,
      body
    );
  }

  ValidateTwoFactorSmsCode(body: ValidateTwoFactorSmsCodeRequestBody) {
    return this.http.post(
      `${this.BASE_URL}/authentication/validate-two-factor-sms`,
      body
    );
  }

  SendConfirmationEmail(details: SendEmailConfirmationRequest) {
    return this.http.post(
      `${this.BASE_URL}/authentication/send-confirmation-email`,
      details
    );
  }

  SendConfirmationEmailCode(body: SendEmailConfirmationCodeRequest) {
    return this.http.post(
      `${this.BASE_URL}/authentication/confirm-email`,
      body
    );
  }

  SendPhoneConfirmation(body: SendPhoneConfirmationRequest) {
    return this.http.post(
      `${this.BASE_URL}/authentication/send-phone-confirmation`,
      body
    );
  }

  /**
   * Used to send the code sent to users device which they type into a field - this is used to confirm there phone number
   * using said code attempt
   */
  ValidatePhoneConfirmationCode(
    body: ValidatePhoneConfirmationCodeRequestBody
  ) {
    return this.http.post(
      `${this.BASE_URL}/authentication/validate-phone-confirmation`,
      body
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
}
