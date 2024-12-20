import { Injectable } from '@angular/core';

interface CookieOptions {
  expires?: number | Date;
  path?: string;
  secure?: boolean;
  sameSite?: 'Strict' | 'Lax' | 'None';
}

@Injectable({
  providedIn: 'root',
})
export class CookieService {

  /**
   * Get the value of a specific cookie by name.
   * @param name The name of the cookie.
   * @returns The value of the cookie, or null if it doesn't exist.
   */
  getCookie(name: string): string | null {
    const nameEQ = encodeURIComponent(name) + "=";
    const cookies = document.cookie.split('; ');

    for (const cookie of cookies) {
      if (cookie.startsWith(nameEQ)) {
        return decodeURIComponent(cookie.substring(nameEQ.length));
      }
    }

    return null;
  }

  /**
   * Set a cookie with a given name, value, and optional options.
   * @param name The name of the cookie.
   * @param value The value of the cookie.
   * @param options Optional settings for the cookie (e.g., expiration, path).
   */
  setCookie(
    name: string,
    value: string,
    options?: CookieOptions
  ): void {
    let cookieString = `${encodeURIComponent(name)}=${encodeURIComponent(value)}`;

    if (options) {
      if (options.expires) {
        const expires = this.formatExpires(options.expires);
        cookieString += `; expires=${expires}`;
      }

      if (options.path) {
        cookieString += `; path=${options.path}`;
      }

      if (options.secure) {
        cookieString += '; secure';
      }

      if (options.sameSite) {
        cookieString += `; samesite=${options.sameSite}`;
      }
    }

    document.cookie = cookieString;
  }

  /**
   * Delete a cookie by name.
   * @param name The name of the cookie to delete.
   * @param path Optional path where the cookie is available (must match the original cookie).
   */
  deleteCookie(name: string, path?: string): void {
    this.setCookie(name, '', { expires: -1, path });
  }

  /**
   * Helper function to format the expiration date.
   * @param expires Expiration value as number of days or Date object.
   * @returns The expiration date string in UTC format.
   */
  private formatExpires(expires: number | Date): string {
    if (expires instanceof Date) {
      return expires.toUTCString();
    } else if (typeof expires === 'number') {
      return new Date(Date.now() + expires * 864e5).toUTCString();
    }
    return '';
  }
}
