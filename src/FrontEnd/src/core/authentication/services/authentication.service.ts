import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseService } from '../../http/services/BaseService.service';
import { LoginResponse, LoginCredentials, SmsCodeRequest, SmsCodeResponse } from '../types';
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

  SendSmsCode(details : SmsCodeRequest): Observable<SmsCodeResponse> {
    return this.post(`${this.BASE_URL}/authentication/resend-two-factor-sms-authentication`, details);
  }

  Logout() {
    this.jwtService.clearTokens();
    this.router.navigate(['/authentication/login']);
  }
}
