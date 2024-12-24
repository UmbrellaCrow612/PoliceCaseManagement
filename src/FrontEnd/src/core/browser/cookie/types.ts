/**
 * Interface defining all available cookie attributes and options
 */
export interface CookieOptions {
  /**
   * Expiration date of the cookie
   * Can be specified as:
   * - Number of days from now
   * - Specific Date object
   */
  expires?: number | Date;

  /**
   * Maximum age of the cookie in seconds
   * Alternative to expires, provides a relative expiration time
   * @example 60 * 60 * 24 // 24 hours
   */
  maxAge?: number;

  /**
   * Path on the server where the cookie is valid
   * @default '/'
   */
  path?: string;

  /**
   * Domain for which the cookie is valid
   * If not specified, defaults to the current domain
   * Must be same domain or parent domain of current site
   * @example '.example.com'
   */
  domain?: string;

  /**
   * Indicates if the cookie should only be sent over HTTPS
   * Strongly recommended for sensitive data
   * @default true
   */
  secure?: boolean;

  /**
   * Controls how the cookie is sent with cross-site requests
   * - 'Strict': Cookie only sent in first-party context
   * - 'Lax': Cookie sent in first-party context and specific cross-site navigation's
   * - 'None': Cookie sent in all contexts (requires secure=true)
   * @default 'Lax'
   */
  sameSite?: 'Strict' | 'Lax' | 'None';

  /**
   * Prevents JavaScript access to the cookie
   * Can only be set by the server, not by client-side JavaScript
   * @note This is a server-side option and won't work in client-side JavaScript
   */
  httpOnly?: boolean;

  /**
   * Suggests the priority of the cookie to browsers
   * Affects which cookies are kept when storage limits are reached
   * @experimental Not supported by all browsers
   */
  priority?: 'Low' | 'Medium' | 'High';

  /**
   * Indicates if this is a partitioned cookie under the CHIPS proposal
   * Used for third-party cookie isolation
   * @experimental Part of the CHIPS (Cookies Having Independent Partitioned State) proposal
   */
  partitioned?: boolean;

  /**
   * Additional size constraints for the cookie
   * @note These are implementation details, not directly settable
   */
  readonly size?: {
    /** Maximum name length in bytes */
    maxNameLength: 255;
    /** Maximum value length in bytes */
    maxValueLength: 4096;
    /** Maximum total size including name, value, and attributes */
    maxTotalSize: 4096;
  };

  /**
   * Indicates if the cookie should be encrypted
   * @experimental Support varies by browser
   */
  encrypt?: boolean;

  /**
   * Preferred expiration handling
   * - 'session': Cookie expires when browser session ends
   * - 'permanent': Cookie persists across sessions
   */
  persistence?: 'session' | 'permanent';

  /**
   * Cross-origin settings
   * @experimental Future specification support
   */
  crossOrigin?: {
    /** Allow specific origins to access this cookie */
    allowedOrigins?: string[];
    /** Access control policy */
    policy?: 'same-origin' | 'same-site' | 'cross-origin';
  };

  /**
   * Custom validation rules for the cookie
   */
  validation?: {
    /** Minimum length for cookie value */
    minLength?: number;
    /** Maximum length for cookie value */
    maxLength?: number;
    /** Regular expression pattern the value must match */
    pattern?: RegExp;
    /** Custom validation function */
    validator?: (value: string) => boolean;
  };
}

/**
 * Cookie attribute constraints as defined by RFC 6265
 * and various browser implementations
 */
export const COOKIE_CONSTRAINTS = {
    /** Maximum number of cookies per domain */
    MAX_COOKIES_PER_DOMAIN: 50,
    /** Maximum size of all cookies per domain (in bytes) */
    MAX_SIZE_PER_DOMAIN: 4096,
    /** Maximum age in seconds (about 400 days) */
    MAX_AGE: 34560000,
    /** Characters not allowed in cookie names */
    FORBIDDEN_NAME_CHARS: /[^\x21\x23-\x2B\x2D-\x3A\x3C-\x5B\x5D-\x7E]/,
    /** Characters that must be encoded in values */
    ENCODE_VALUE_CHARS: /[^\x21\x23-\x2B\x2D-\x3A\x3C-\x5B\x5D-\x7E]/
  } as const;