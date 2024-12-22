import { Injectable } from '@angular/core';

interface CookieOptions {
  expires?: number | Date;
  path?: string;
  domain?: string;
  secure?: boolean;
  sameSite?: 'Strict' | 'Lax' | 'None';
  httpOnly?: boolean;
  priority?: 'Low' | 'Medium' | 'High';
  partitioned?: boolean;
  maxAge?: number;
}

@Injectable({
  providedIn: 'root',
})
export class CookieService {
  private defaultOptions: CookieOptions = {
    path: '/',
    sameSite: 'Lax',
    secure: true
  };

  /**
   * Configure default options for all cookie operations
   * @param options Default cookie options to be applied
   */
  setDefaultOptions(options: CookieOptions): void {
    this.defaultOptions = { ...this.defaultOptions, ...options };
  }

  /**
   * Get the value of a specific cookie by name
   * @param name The name of the cookie
   * @returns The value of the cookie, or null if it doesn't exist
   */
  getCookie(name: string): string | null {
    try {
      const nameEQ = encodeURIComponent(name) + "=";
      const cookies = document.cookie.split('; ');

      for (const cookie of cookies) {
        if (cookie.startsWith(nameEQ)) {
          return decodeURIComponent(cookie.substring(nameEQ.length));
        }
      }
    } catch (error) {
      console.error('Error reading cookie:', error);
    }
    return null;
  }

  /**
   * Get all cookies as a key-value object
   * @returns Object containing all cookies
   */
  getAllCookies(): Record<string, string> {
    const cookies: Record<string, string> = {};
    try {
      document.cookie.split('; ').forEach(cookie => {
        const [name, value] = cookie.split('=').map(decodeURIComponent);
        cookies[name] = value;
      });
    } catch (error) {
      console.error('Error reading cookies:', error);
    }
    return cookies;
  }

  /**
   * Check if a cookie exists
   * @param name The name of the cookie
   * @returns boolean indicating if cookie exists
   */
  hasCookie(name: string): boolean {
    return this.getCookie(name) !== null;
  }

  /**
   * Set a cookie with a given name, value, and optional options
   * @param name The name of the cookie
   * @param value The value of the cookie
   * @param options Optional settings for the cookie
   */
  setCookie(
    name: string,
    value: string,
    options?: CookieOptions
  ): void {
    try {
      const mergedOptions = { ...this.defaultOptions, ...options };
      let cookieString = `${encodeURIComponent(name)}=${encodeURIComponent(value)}`;

      if (mergedOptions.expires) {
        const expires = this.formatExpires(mergedOptions.expires);
        cookieString += `; expires=${expires}`;
      }

      if (mergedOptions.maxAge !== undefined) {
        cookieString += `; max-age=${mergedOptions.maxAge}`;
      }

      if (mergedOptions.path) {
        cookieString += `; path=${mergedOptions.path}`;
      }

      if (mergedOptions.domain) {
        cookieString += `; domain=${mergedOptions.domain}`;
      }

      if (mergedOptions.secure) {
        cookieString += '; secure';
      }

      if (mergedOptions.sameSite) {
        cookieString += `; samesite=${mergedOptions.sameSite}`;
      }

      if (mergedOptions.priority) {
        cookieString += `; priority=${mergedOptions.priority}`;
      }

      if (mergedOptions.partitioned) {
        cookieString += '; partitioned';
      }

      document.cookie = cookieString;
    } catch (error) {
      console.error('Error setting cookie:', error);
    }
  }

  /**
   * Delete a cookie by name
   * @param name The name of the cookie to delete
   * @param options Optional settings that must match the original cookie
   */
  deleteCookie(name: string, options?: Partial<CookieOptions>): void {
    const mergedOptions = { ...this.defaultOptions, ...options };
    this.setCookie(name, '', { 
      ...mergedOptions,
      expires: new Date(0) 
    });
  }

  /**
   * Delete all cookies
   * @param options Optional settings that must match the original cookies
   */
  deleteAllCookies(options?: Partial<CookieOptions>): void {
    const cookies = this.getAllCookies();
    Object.keys(cookies).forEach(name => {
      this.deleteCookie(name, options);
    });
  }

  /**
   * Helper function to format the expiration date
   * @param expires Expiration value as number of days or Date object
   * @returns The expiration date string in UTC format
   */
  private formatExpires(expires: number | Date): string {
    if (expires instanceof Date) {
      return expires.toUTCString();
    } else if (typeof expires === 'number') {
      return new Date(Date.now() + expires * 864e5).toUTCString();
    }
    return '';
  }

  /**
   * Update a cookie's value while preserving its existing options
   * @param name The name of the cookie
   * @param value The new value
   * @returns boolean indicating if update was successful
   */
  updateCookie(name: string, value: string): boolean {
    try {
      const currentValue = this.getCookie(name);
      if (currentValue !== null) {
        this.setCookie(name, value);
        return true;
      }
      return false;
    } catch (error) {
      console.error('Error updating cookie:', error);
      return false;
    }
  }
}