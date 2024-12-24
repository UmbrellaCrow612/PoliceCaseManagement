import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BaseService } from "../../http/services/BaseService.service";
import { JwtResponse, LoginCredentials } from "../types";
import DevelopmentConfig from "../../../environments/development";
import { catchError, interval, Observable, of, Subscription, switchMap } from "rxjs";
import { JwtService } from "./jwt.service";

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService extends BaseService {
    
  private readonly BASE_URL = DevelopmentConfig.BaseUrls.authenticationBaseUrl;
  private readonly tokenCheckInterval = 5000; 
  private subscription: Subscription | null = null;

  constructor(protected override http: HttpClient, private jwtService : JwtService) {
    super(http);
  }

  Login(credentials : LoginCredentials) : Observable<JwtResponse> {
    return this.post(`${this.BASE_URL}/authentication/login`, credentials);
  }

  StartTokenValidationThroughoutLifetime(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    this.subscription = interval(this.tokenCheckInterval)
      .pipe(
        switchMap(() => {
          console.log('StartTokenValidationThroughoutLifetime ran.');

          const rawToken = this.jwtService.getRawJwtToken();
          console.log('rawToken', rawToken);
          
          const rawRefreshToken = this.jwtService.getRawRefreshToken();
          console.log('rawRefreshToken', rawRefreshToken);

          if (rawToken && rawRefreshToken && this.jwtService.IsJwtTokenValid(rawToken)) {
            console.log('Sent request for new tokens');
            this.jwtService.refreshToken(rawRefreshToken);
          }

          return of(null); 
        }),
        catchError((err) => {
          console.error('Token validation/refresh failed', err);
          return of(null); 
        })
      )
      .subscribe();
  }

  StopTokenValidation(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
      this.subscription = null;
    }
  }
}
