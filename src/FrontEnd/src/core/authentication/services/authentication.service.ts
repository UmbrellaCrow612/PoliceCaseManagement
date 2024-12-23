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


}
