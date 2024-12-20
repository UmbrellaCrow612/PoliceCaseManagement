import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { LoginCredentials } from "../types";
import { jwtDecode, JwtPayload } from "jwt-decode";
import { CookieService } from "../../cookie/services/cookie.service";

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
    
  private readonly COOKIE_NAME = "jt";

  constructor(private http: HttpClient, private cookieService: CookieService) {}

  async Login(loginCredentials: LoginCredentials) {
    // Implement login functionality here
  }

  Logout() {
    console.log("Logout ran");
    this.cookieService.deleteCookie(this.COOKIE_NAME); 
  }

  async RefreshToken() {
    console.log("Refreshing token ran");
    // Implement token refresh functionality here
  }

  DecodeJwtToken(token: string): JwtPayload {
    return jwtDecode(token);
  }

  GetJwtToken(): string | null {
    return this.cookieService.getCookie(this.COOKIE_NAME);
  }

  private IsTokenValid() {
    console.log("Token valid ran");
    // Implement token validation functionality here
  }

  public SetJwtToken(token: string, expirationMinutes: number): void {
    const expirationDate = new Date();
    expirationDate.setTime(expirationDate.getTime() + expirationMinutes * 60 * 1000);
    
    const cookieOptions = {
      expires: expirationDate,
      path: '/',
      secure: true,
      sameSite: 'Strict' as 'Strict', 
    };
  
    this.cookieService.setCookie('jwt', token, cookieOptions);  
  }
  
}
