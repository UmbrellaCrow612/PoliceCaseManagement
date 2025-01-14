import { Injectable } from '@angular/core';
import { CookieOptions } from '../types';

@Injectable({
  providedIn: 'root',
})
export class CookieService {
  getCookie(name: string): string | null {
    try {
      const nameEQ = encodeURIComponent(name) + '=';
      const decodedCookies = decodeURIComponent(document.cookie);
      const cookies = decodedCookies.split('; ');

      for (const cookie of cookies) {
        if (!cookie.includes('=')) continue;

        if (cookie.trim().startsWith(nameEQ)) {
          const value = cookie.substring(nameEQ.length);

          try {
            return decodeURIComponent(value);
          } catch (decodeError) {
            console.warn(
              `Failed to decode cookie value for ${name}:`,
              decodeError
            );
            return null;
          }
        }
      }
    } catch (error) {
      console.error('Error reading cookie:', error);

      if (error instanceof Error && error.message.includes('Cookie name')) {
        throw error;
      }
    }

    return null;
  }

  setCookie(name: string, value: string, options?: CookieOptions): void {
    try {
      const defaultOptions: CookieOptions = {
        SameSite: 'Lax',
        Secure: true,
        Path: '/',
      };

      const finalOptions = { ...defaultOptions, ...options };

      let cookieString = `${encodeURIComponent(name)}=${encodeURIComponent(
        value
      )}; `;

      // Build the cookie string based on the merged options
      if (typeof finalOptions.SameSite === 'string') {
        cookieString += `SameSite=${finalOptions.SameSite}; `;
      }
      if (finalOptions.Secure) {
        cookieString += `Secure; `;
      }
      if (typeof finalOptions.Path === 'string') {
        cookieString += `Path=${finalOptions.Path}; `;
      }
      if (typeof finalOptions.Domain === 'string') {
        cookieString += `Domain=${finalOptions.Domain}; `;
      }
      if (
        typeof finalOptions.Expires === 'string' ||
        finalOptions.Expires instanceof Date
      ) {
        const expires =
          finalOptions.Expires instanceof Date
            ? finalOptions.Expires.toUTCString()
            : finalOptions.Expires;
        cookieString += `Expires=${expires}; `;
      }
      if (typeof finalOptions.MaxAge === 'number') {
        cookieString += `Max-Age=${finalOptions.MaxAge}; `;
      }

      document.cookie = cookieString.trim();
    } catch (error) {
      console.error('Error setting cookie:', error);
      throw error;
    }
  }

  deleteCookie(name: string, path: string = '/', domain?: string): void {
    let cookieString = `${name}=; Expires=Thu, 01 Jan 1970 00:00:00 UTC; Path=${path};`;
    if (domain) {
      cookieString += ` Domain=${domain};`;
    }
    document.cookie = cookieString;
  }
}
