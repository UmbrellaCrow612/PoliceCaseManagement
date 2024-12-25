import { Injectable } from '@angular/core';
import { CookieOptions } from '../types';
import { COOKIE_CONSTRAINTS, isCookieOptions } from '../validator';

@Injectable({
  providedIn: 'root',
})
export class CookieService {
  private defaultOptions: CookieOptions = {
    secure: true,
    sameSite: 'Lax',
    path: '/',

    priority: 'Medium',
    persistence: 'permanent',

    validation: {
      maxLength: 4096,
      minLength: 1,
      validator: (value: string) => {
        return value.length > 0 && value.length <= 4096;
      },
    },

    crossOrigin: {
      policy: 'same-origin',
    },
  };

  /**
   * Configure default options for all cookie operations
   * @param options Default cookie options to be applied
   * @throws Error if invalid options are provided
   */
  setDefaultOptions(options: Partial<CookieOptions>): void {
    if (!isCookieOptions(options)) {
      throw new Error('Invalid cookie options provided');
    }

    if (
      options.secure === false &&
      (options.sameSite === 'None' || this.defaultOptions.sameSite === 'None')
    ) {
      throw new Error("secure must be true when sameSite is 'None'");
    }

    const newOptions: CookieOptions = {
      ...this.defaultOptions,
      ...options,
      secure: options.secure ?? this.defaultOptions.secure,
      sameSite:
        this.validateSameSite(options.sameSite) ?? this.defaultOptions.sameSite,
      validation: {
        ...this.defaultOptions.validation,
        ...options.validation,
      },
      crossOrigin: {
        ...this.defaultOptions.crossOrigin,
        ...options.crossOrigin,
      },
    };

    this.validateOptions(newOptions);

    this.defaultOptions = newOptions;
  }

  /**
   * Validates SameSite attribute value
   * @param sameSite Value to validate
   * @returns Valid SameSite value or undefined
   */
  private validateSameSite(
    sameSite?: 'Strict' | 'Lax' | 'None'
  ): 'Strict' | 'Lax' | 'None' | undefined {
    if (!sameSite) return undefined;
    if (!['Strict', 'Lax', 'None'].includes(sameSite)) {
      throw new Error("sameSite must be 'Strict', 'Lax', or 'None'");
    }
    return sameSite;
  }

  /**
   * Validates complete options object
   * @param options Options to validate
   * @throws Error if options are invalid
   */
  private validateOptions(options: CookieOptions): void {
    if (options.expires && options.maxAge) {
      throw new Error('Cannot set both expires and maxAge');
    }

    if (options.maxAge && options.maxAge <= 0 || options.maxAge === 0) {
      throw new Error('maxAge must be positive');
    }

    if (options.domain) {
      if (!this.isValidDomain(options.domain)) {
        throw new Error('Invalid domain provided');
      }
    }

    if (options.path && !options.path.startsWith('/')) {
      throw new Error('Path must start with /');
    }

  }

  /**
   * Validates if a domain is valid for cookie setting
   * @param domain Domain to validate
   * @returns boolean indicating if domain is valid
   */
  private isValidDomain(domain: string): boolean {
    if (!/^[a-zA-Z0-9-_.]+$/.test(domain)) {
      return false;
    }

    const currentDomain = window.location.hostname;
    return domain === currentDomain || currentDomain.endsWith('.' + domain);
  }

  /**
   * Get the value of a specific cookie by name
   * @param name The name of the cookie
   * @returns The value of the cookie, or null if it doesn't exist
   * @throws Error if name is invalid
   */
  getCookie(name: string): string | null {
    if (!name || typeof name !== 'string') {
      throw new Error('Cookie name must be a non-empty string');
    }

    if (COOKIE_CONSTRAINTS.FORBIDDEN_NAME_CHARS.test(name)) {
      throw new Error('Cookie name contains invalid characters');
    }

    try {
      const nameEQ = encodeURIComponent(name) + '=';
      const decodedCookies = decodeURIComponent(document.cookie);
      const cookies = decodedCookies.split('; ');

      if (cookies.length === 1 && cookies[0] === '') {
        return null;
      }

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
   * @throws Error if name/value are invalid or options are invalid
   */
  setCookie(
    name: string,
    value: string,
    options?: Partial<CookieOptions>
  ): void {
    if (!name || typeof name !== 'string') {
      throw new Error('Cookie name must be a non-empty string');
    }

    if (typeof value !== 'string') {
      throw new Error('Cookie value must be a string');
    }

    if (COOKIE_CONSTRAINTS.FORBIDDEN_NAME_CHARS.test(name)) {
      throw new Error('Cookie name contains invalid characters');
    }

    if (value.length > COOKIE_CONSTRAINTS.MAX_SIZE_PER_DOMAIN) {
      throw new Error('Cookie value exceeds maximum size');
    }

    try {
      const mergedOptions = this.getMergedOptions(options);

      this.validateCombinedOptions(mergedOptions);

      const cookieParts: string[] = [
        `${encodeURIComponent(name)}=${encodeURIComponent(value)}`,
      ];

      this.appendOptionsToCookie(cookieParts, mergedOptions);

      const cookieString = cookieParts.join('; ');

      if (cookieString.length > COOKIE_CONSTRAINTS.MAX_SIZE_PER_DOMAIN) {
        throw new Error('Total cookie size exceeds maximum allowed size');
      }

      document.cookie = cookieString;

      if (!this.hasCookie(name)) {
        throw new Error('Failed to set cookie');
      }
    } catch (error) {
      console.error('Error setting cookie:', error);
      throw error; 
    }
  }

  /**
   * Append validated options to cookie parts array
   */
  private appendOptionsToCookie(parts: string[], options: CookieOptions): void {
    if (options.expires) {
      parts.push(`expires=${this.formatExpires(options.expires)}`);
    }

    if (options.maxAge !== undefined) {
      parts.push(`max-age=${options.maxAge}`);
    }

    if (options.path) {
      parts.push(`path=${options.path}`);
    }

    if (options.domain) {
      parts.push(`domain=${options.domain}`);
    }

    if (options.secure) {
      parts.push('secure');
    }

    if (options.sameSite) {
      parts.push(`samesite=${options.sameSite}`);
    }

    if (options.priority) {
      parts.push(`priority=${options.priority}`);
    }

    if (options.partitioned) {
      parts.push('partitioned');
    }
  }

  /**
   * Validate complete set of options after merging
   */
  private validateCombinedOptions(options: CookieOptions): void {
    // Check for expires + maxAge conflict
    if (options.expires && options.maxAge) {
      throw new Error('Cannot set both expires and maxAge');
    }

    // Validate maxAge
    if (options.maxAge) {
      if (options.maxAge <= 0 || options.maxAge > COOKIE_CONSTRAINTS.MAX_AGE) {
        throw new Error(
          `maxAge must be between 1 and ${COOKIE_CONSTRAINTS.MAX_AGE}`
        );
      }
    }

    // Validate domain
    if (options.domain && !this.isValidDomain(options.domain)) {
      throw new Error('Invalid domain');
    }

    // Validate path
    if (options.path && !options.path.startsWith('/')) {
      throw new Error('Path must start with /');
    }

    // Validate sameSite + secure combination
    if (options.sameSite === 'None' && !options.secure) {
      throw new Error("secure must be true when sameSite is 'None'");
    }
  }

  /**
   * Merge provided options with defaults and validate
   */
  private getMergedOptions(options?: Partial<CookieOptions>): CookieOptions {
    const mergedOptions = {
      ...this.defaultOptions,
      ...options,
    };

    if (mergedOptions.sameSite === 'None') {
      mergedOptions.secure = true;
    }

    return mergedOptions;
  }

  /**
   * Delete a cookie by name
   * @param name The name of the cookie to delete
   * @param options Optional settings that must match the original cookie's options
   * @throws Error if name is invalid or cookie deletion fails
   * @returns boolean indicating if cookie was successfully deleted
   */
  deleteCookie(name: string, options?: Partial<CookieOptions>): boolean {
    if (!name || typeof name !== 'string') {
      throw new Error('Cookie name must be a non-empty string');
    }

    if (COOKIE_CONSTRAINTS.FORBIDDEN_NAME_CHARS.test(name)) {
      throw new Error('Cookie name contains invalid characters');
    }

    try {
      if (!this.hasCookie(name)) {
        return true; 
      }

      const existingOptions = this.getCookieOptions(name);
      const mergedOptions: CookieOptions = {
        ...this.defaultOptions,
        ...existingOptions,
        ...options,
        expires: new Date(0), // Past date forces deletion
        maxAge: 0, // Alternative way to force deletion
        path:
          options?.path || existingOptions?.path || this.defaultOptions.path,
        domain:
          options?.domain ||
          existingOptions?.domain ||
          this.defaultOptions.domain,
      };

      this.setCookie(name, '', mergedOptions);

      const stillExists = this.hasCookie(name);
      if (stillExists) {
        try {
          document.cookie = `${encodeURIComponent(
            name
          )}=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/`;

          if (mergedOptions.domain) {
            document.cookie = `${encodeURIComponent(
              name
            )}=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/; domain=${
              mergedOptions.domain
            }`;
          }
        } catch (retryError) {
          console.error('Error during cookie deletion retry:', retryError);
        }
      }

      const deleted = !this.hasCookie(name);

      return deleted;
    } catch (error) {
      console.error('Error deleting cookie:', error);
      throw new Error(
        `Failed to delete cookie ${name}: ${
          error instanceof Error ? error.message : 'Unknown error'
        }`
      );
    }
  }

  /**
   * Helper method to attempt to extract existing cookie options
   * @private
   */
  private getCookieOptions(name: string): Partial<CookieOptions> | null {
    try {
      const cookies = document.cookie.split('; ');
      const cookieStr = cookies.find((c) =>
        c.startsWith(`${encodeURIComponent(name)}=`)
      );

      if (!cookieStr) return null;

      const options: Partial<CookieOptions> = {};

      // Parse existing cookie attributes
      if (cookieStr.includes('path=')) {
        const path = cookieStr.match(/path=([^;]+)/)?.[1];
        if (path) options.path = path;
      }

      if (cookieStr.includes('domain=')) {
        const domain = cookieStr.match(/domain=([^;]+)/)?.[1];
        if (domain) options.domain = domain;
      }

      if (cookieStr.includes('secure')) {
        options.secure = true;
      }

      if (cookieStr.includes('samesite=')) {
        const sameSite = cookieStr.match(/samesite=([^;]+)/)?.[1] as
          | 'Strict'
          | 'Lax'
          | 'None';
        if (sameSite) options.sameSite = sameSite;
      }

      return options;
    } catch {
      return null;
    }
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
}
