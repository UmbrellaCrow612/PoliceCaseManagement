import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { LoginCredentials } from "../types";
import { jwtDecode, JwtPayload } from "jwt-decode";
import CookieNames from "../../browser/cookie/constants/names";
import { CookieService } from "../../browser/cookie/services/cookie.service";

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
    
  private readonly COOKIE_NAME = CookieNames.JWT;

  constructor(private http: HttpClient, private cookieService: CookieService) {}

  async Login(loginCredentials: LoginCredentials) {
    // Implement login functionality here
  }

  Logout() {
    // Implement login functionality here
  }

  async RefreshToken() {
    // Implement token refresh functionality here
  }

  DecodeJwtToken(token: string): JwtPayload {
    return jwtDecode(token);
  }

  GetJwtToken(): string | null {
    return this.cookieService.getCookie(this.COOKIE_NAME);
  }


  public SetJwtToken(token: string, expirationMinutes: number): void {
    // Implement login functionality here
  }
}
