import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { JwtPayload } from '../types';
import { CookieService } from '../../browser/cookie/services/cookie.service';
import CookieNames from '../../browser/cookie/constants/names';

@Injectable({
  providedIn: 'root',
})
export class JwtService {
  /**
   * The name of the cookie where the JWT token is stored.
   * @private
   * @readonly
   */
  private readonly COOKIE_NAME = CookieNames.JWT;

  /**
   * The name of the cookie where the JWT token is stored.
   * @private
   * @readonly
   */
  private readonly REFRESH_NAME = CookieNames.REFRESH_TOKEN;

  constructor(private cookieService: CookieService) {}

  /**
   * Validates whether a given JWT token is still valid based on its expiration time.
   * @param {string} token The JWT token to validate.
   * @returns {boolean} True if the token is valid; otherwise, false.
   */
  IsJwtTokenValid(token: string): boolean {
    const claims = this.decodeToken<JwtPayload>(token);

    if (claims.exp) {
      const expiryDate = new Date(claims.exp * 1000);
      const currentDate = new Date();
      return expiryDate > currentDate;
    }

    return false;
  }

  /**
   * Retrieves the JWT token stored in the cookie.
   * @returns {string | null} The JWT token if it exists; otherwise, null.
   */
  getRawJwtToken(): string | null {
    return this.cookieService.getCookie(this.COOKIE_NAME);
  }

  /**
   * Retrieves the Refresh token stored in the cookie.
   * @returns {string | null} The Refresh token if it exists; otherwise, null.
   */
  getRawRefreshToken(): string | null {
    return this.cookieService.getCookie(this.REFRESH_NAME);
  }

  refreshToken(refreshToken: string) {
    // do refresh logic and set the new tokens in the store , non blocking extra task
  }

  setTokens() {
    // override the values of access token and refresh token in cookies
  }

  /**
   * Decodes a JWT token and returns the payload.
   * @template T The expected type of the JWT payload, extends JwtPayload.
   * @param {string} token The JWT token to decode.
   * @returns {T} The decoded token payload.
   * @throws {Error} If the token is invalid or cannot be decoded.
   */
  decodeToken<T extends JwtPayload>(token: string): T {
    try {
      return jwtDecode<T>(token);
    } catch (error) {
      throw new Error(
        'Failed to decode JWT token: ' + (error as Error).message
      );
    }
  }
}
