import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseService } from '../../http/services/BaseService.service';
import { JwtResponse, LoginCredentials } from '../types';
import env from '../../../environments/environment';
import { Observable } from 'rxjs';
import { JwtService } from './jwt.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService extends BaseService {
  private readonly BASE_URL = env.BaseUrls.authenticationBaseUrl;

  constructor(protected override http: HttpClient, private jwtService: JwtService, private router: Router) {
    super(http);
  }

  Login(credentials: LoginCredentials): Observable<JwtResponse> {
    return this.post(`${this.BASE_URL}/authentication/login`, credentials);
  }

  Logout(){
    this.jwtService.clearTokens();
    this.router.navigate(['/login']);
  }
}
