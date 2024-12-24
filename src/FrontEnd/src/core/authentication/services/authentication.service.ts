import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import CookieNames from "../../browser/cookie/constants/names";
import { CookieService } from "../../browser/cookie/services/cookie.service";
import { BaseService } from "../../http/services/BaseService.service";
import { JwtResponse, LoginCredentials } from "../types";
import DevelopmentConfig from "../../../environments/development";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService extends BaseService {
    
  private readonly COOKIE_NAME = CookieNames.JWT;
  private readonly BASE_URL = DevelopmentConfig.BaseUrls.authenticationBaseUrl;

  constructor(protected override http: HttpClient) {
    super(http);
  }

  Login(credentials : LoginCredentials) : Observable<JwtResponse> {
    return this.post(`${this.BASE_URL}/authentication/login`, credentials);
  }

  // need to be implemented but would be called on mount of the app - if no token then don't start the logic to check etc
  StartTokenValidationThroughoutLifetime(){
    // idea
    // interval(this.tokenCheckInterval)
    // .pipe(
    //   switchMap(() => {
    //     const token = this.getToken();
    //     if (token && this.isTokenExpired(token)) {
    //       return this.refreshToken();
    //     }
    //     return of(null); // No request if the token is valid
    //   }),
    //   catchError((err) => {
    //     console.error('Token validation/refresh failed', err);
    //     this.logout();
    //     return of(null); // Handle errors gracefully
    //   })
    // )
    // .subscribe();
  }


}
