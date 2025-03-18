import { Injectable } from '@angular/core';
import { BaseService } from '../../http/services/BaseService.service';
import { Subscription, timer } from 'rxjs';
import env from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { AuthenticationService } from './authentication.service';

@Injectable({
  providedIn: 'root',
})
export class JwtService extends BaseService {
  constructor(
    httpClient: HttpClient,
    private authService: AuthenticationService
  ) {
    super(httpClient);
  }

  validationSubscriptionTimer = timer(
    env.JWTTokenValidationInitialWaitTimeInMilliSeconds,
    env.JwtTokenValidationPeriodInMinutes
  );
  validationSubscription: null | Subscription = null;
  BASE_URL = env.BaseUrls.authenticationBaseUrl;

  /**
   * Service which should start at the beginning of the app life cycle - which will periodically refresh jwt
   * tokens in the background when they are about to become stale
   * if it cannot or can no longer if will automatically log out the user.
   */
  startTokenValidationThroughoutLifeTimeOfApp() {
    this.validationSubscription = this.validationSubscriptionTimer.subscribe(
      () => {
        console.log('JWT token validation running');

        // Since its Http only cookie we can not really read it's value etc
        // so we will just hit the fresh endpoint every x amount of  minutes until we get a 401
        // which means it failed and log out

        this.get(`${this.BASE_URL}/authentication/refresh-token`).subscribe({
          next: () => {
            console.log('JWT token refreshed');
          },
          error: () => {
            console.warn('JWT token stale');
            this.authService.Logout();
          },
        });
      }
    );
  }

  /**
   * Stop the startTokenValidationThroughoutLifeTimeOfApp and destroy any state
   */
  stopTokenValidationThroughoutLifeTimeOfApp() {
    if (this.validationSubscription) {
      this.validationSubscription.unsubscribe();
      console.log('JWT token validation finished ');
    }
  }
}
