import { Injectable } from '@angular/core';
import { BaseService } from '../../http/services/BaseService.service';
import { filter, Subscription, timer } from 'rxjs';
import env from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { AuthenticationService } from './authentication.service';
import { appPaths } from '../../app/constants/appPaths';
import { Location } from '@angular/common';
import { NavigationEnd, Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class JwtService extends BaseService {
  constructor(httpClient: HttpClient, private router: Router) {
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
   * NOTE: dose not run if inside of authentication module as they would not be authenticated
   */
  startTokenValidationThroughoutLifeTimeOfApp() {
    this.router.events.subscribe(() => {
      if (!window.location.href.includes(appPaths.AUTHENTICATION)) {
        if (this.validationSubscription) {
          this.validationSubscription.unsubscribe();
        }
        this.validationSubscription =
          this.validationSubscriptionTimer.subscribe(() => {
            console.log('JWT token validation running');

            this.get(`${this.BASE_URL}/authentication/refresh-token`).subscribe(
              {
                next: () => {
                  console.log('JWT token refreshed');
                },
                error: () => {
                  console.warn('JWT token stale');
                },
              }
            );
          });
      } else {
        this.stopTokenValidation();
      }
    });
  }

  stopTokenValidation() {
    if (this.validationSubscription) {
      this.validationSubscription.unsubscribe();
    }
  }
}
