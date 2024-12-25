import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { JwtPayload } from '../types';
import { CookieService } from '../../browser/cookie/services/cookie.service';
import CookieNames from '../../browser/cookie/constants/names';
import { BaseService } from '../../http/services/BaseService.service';
import { HttpClient } from '@angular/common/http';
import env from '../../../environments/enviroment';

@Injectable({
  providedIn: 'root',
})
export class JwtService extends BaseService {
  /**
   * The base url this service will use to make requests.
   * @private
   * @readonly
   */
  private readonly BASE_URL = env.BaseUrls.authenticationBaseUrl;

  /**
   * The name of the cookie where the JWT token is stored.
   * @private
   * @readonly
   */
  private readonly JWT_NAME = CookieNames.JWT;

  /**
   * The name of the cookie where the JWT token is stored.
   * @private
   * @readonly
   */
  private readonly REFRESH_NAME = CookieNames.REFRESH_TOKEN;

  constructor(
    protected override http: HttpClient,
    private cookieService: CookieService
  ) {
    super(http);
  }

  /**
   * Validates whether a given JWT token is still valid based on its expiration time.
   * The type `T` passed in should match the shape of the token you are passing in.
   * For example, if your token includes custom claims, extend `JwtPayload` with those claims.
   * @param {string} token The raw JWT token to validate.
   * @returns {boolean} True if the token is valid (not expired); otherwise, false.
   */
  IsJwtTokenValid<T extends JwtPayload>(rawToken: string): boolean {
    try {
      const claims = this.decodeToken<T>(rawToken);

      if (claims.exp) {
        const expiryDate = new Date(claims.exp * 1000);
        const currentDate = new Date();
        return expiryDate > currentDate;
      }
    } catch (error) {
      console.error('Error decoding token:', error);
      return false;
    }

    return false;
  }

  /**
   * Retrieves the JWT token stored in the cookie.
   * @returns {string | null} The JWT token if it exists; otherwise, null.
   */
  getRawJwtToken(): string | null {
    return this.cookieService.getCookie(this.JWT_NAME);
  }

  /**
   * Retrieves the Refresh token stored in the cookie.
   * @returns {string | null} The Refresh token if it exists; otherwise, null.
   */
  getRawRefreshToken(): string | null {
    return this.cookieService.getCookie(this.REFRESH_NAME);
  }

  refreshToken(refreshToken: string) {
    const body = { refreshToken };

    this.post<{ accessToken: string; refreshToken: string }>(
      `${this.BASE_URL}/authentication/refresh-token`,
      body
    ).subscribe({
      next: (response) => {
        this.setTokens(response.accessToken, response.refreshToken);
      },
      error: (err) => {
        console.error('Error refreshing token:', err);
      },
    });
  }

  /**
   * Set the access and refresh tokens in the cookie.
   */
  setTokens(rawAccessToken: string, rawRefreshToken: string) {
    this.cookieService.deleteCookie(this.JWT_NAME);
    this.cookieService.deleteCookie(this.REFRESH_NAME);

    this.cookieService.setCookie(this.JWT_NAME, rawAccessToken);
    this.cookieService.setCookie(this.REFRESH_NAME, rawRefreshToken);
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