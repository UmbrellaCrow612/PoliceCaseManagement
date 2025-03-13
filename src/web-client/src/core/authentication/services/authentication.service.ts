import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseService } from '../../http/services/BaseService.service';
import {
  LoginResponse,
  LoginCredentials,
  SmsCodeRequest,
  SmsCodeResponse,
  SendEmailConfirmationRequest,
  SendEmailConfirmationCodeRequest,
  SendPhoneConfirmationRequest,
  ValidatePhoneConfirmationCodeRequestBody,
} from '../types';
import env from '../../../environments/environment';
import { Observable } from 'rxjs';
import { JwtService } from './jwt.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService extends BaseService {
  private readonly BASE_URL = env.BaseUrls.authenticationBaseUrl;

  constructor(
    protected override http: HttpClient,
    private jwtService: JwtService,
    private router: Router
  ) {
    super(http);
  }

  Login(credentials: LoginCredentials): Observable<LoginResponse> {
    return this.post(`${this.BASE_URL}/authentication/login`, credentials);
  }

  SendSmsCode(details: SmsCodeRequest): Observable<SmsCodeResponse> {
    return this.post(
      `${this.BASE_URL}/authentication/resend-two-factor-sms-authentication`,
      details
    );
  }

  ValidateSmsCode(details: SmsCodeRequest): Observable<SmsCodeResponse> {
    return this.post(
      `${this.BASE_URL}/authentication/validate-two-factor-sms-authentication`,
      details
    );
  }

  SendConfirmationEmail(details: SendEmailConfirmationRequest) {
    return this.post(
      `${this.BASE_URL}/authentication/send-confirmation-email`,
      details
    );
  }

  SendConfirmationEmailCode(body: SendEmailConfirmationCodeRequest) {
    return this.post(`${this.BASE_URL}/authentication/confirm-email`, body);
  }

  SendPhoneConfirmation(body: SendPhoneConfirmationRequest) {
    return this.post(
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
    return this.post(
      `${this.BASE_URL}/authentication/validate-phone-confirmation`,
      body
    );
  }

  Logout() {
    this.jwtService.clearTokens();
    this.router.navigate(['/authentication/login']);
  }
}
