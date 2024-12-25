import { CookieOptions } from "./types";

/**
 * Type guard to check if a value is a valid CookieOptions object
 * Validates all properties according to standard cookie specifications
 * and includes support for experimental and future features
 *
 * @param value - Value to check
 * @returns True if value is a valid CookieOptions object
 */
export function isCookieOptions(value: unknown): value is CookieOptions {
  if (!value || typeof value !== 'object') return false;

  const options = value as CookieOptions;

  if (options.expires !== undefined) {
    if (
      !(options.expires instanceof Date) &&
      typeof options.expires !== 'number'
    ) {
      return false;
    }
  }

  if (options.maxAge !== undefined) {
    if (
      typeof options.maxAge !== 'number' ||
      options.maxAge > COOKIE_CONSTRAINTS.MAX_AGE ||
      options.maxAge < 0
    ) {
      return false;
    }
  }

  if (options.path !== undefined && typeof options.path !== 'string') {
    return false;
  }

  if (options.domain !== undefined && typeof options.domain !== 'string') {
    return false;
  }

  if (options.secure !== undefined && typeof options.secure !== 'boolean') {
    return false;
  }

  if (options.httpOnly !== undefined && typeof options.httpOnly !== 'boolean') {
    return false;
  }

  if (
    options.sameSite !== undefined &&
    !['Strict', 'Lax', 'None'].includes(options.sameSite)
  ) {
    return false;
  }

  if (
    options.priority !== undefined &&
    !['Low', 'Medium', 'High'].includes(options.priority)
  ) {
    return false;
  }

  if (
    options.partitioned !== undefined &&
    typeof options.partitioned !== 'boolean'
  ) {
    return false;
  }

  if (options.size !== undefined) {
    if (
      typeof options.size !== 'object' ||
      typeof options.size.maxNameLength !== 'number' ||
      typeof options.size.maxValueLength !== 'number' ||
      typeof options.size.maxTotalSize !== 'number'
    ) {
      return false;
    }
  }

  if (options.encrypt !== undefined && typeof options.encrypt !== 'boolean') {
    return false;
  }

  if (
    options.persistence !== undefined &&
    !['session', 'permanent'].includes(options.persistence)
  ) {
    return false;
  }

  if (options.crossOrigin !== undefined) {
    if (typeof options.crossOrigin !== 'object') return false;

    if (options.crossOrigin.allowedOrigins !== undefined) {
      if (
        !Array.isArray(options.crossOrigin.allowedOrigins) ||
        !options.crossOrigin.allowedOrigins.every(
          (origin) => typeof origin === 'string'
        )
      ) {
        return false;
      }
    }

    if (
      options.crossOrigin.policy !== undefined &&
      !['same-origin', 'same-site', 'cross-origin'].includes(
        options.crossOrigin.policy
      )
    ) {
      return false;
    }
  }

  if (options.validation !== undefined) {
    if (typeof options.validation !== 'object') return false;

    if (
      options.validation.minLength !== undefined &&
      (typeof options.validation.minLength !== 'number' ||
        options.validation.minLength < 0)
    ) {
      return false;
    }

    if (
      options.validation.maxLength !== undefined &&
      (typeof options.validation.maxLength !== 'number' ||
        options.validation.maxLength < 0)
    ) {
      return false;
    }

    if (
      options.validation.pattern !== undefined &&
      !(options.validation.pattern instanceof RegExp)
    ) {
      return false;
    }

    if (
      options.validation.validator !== undefined &&
      typeof options.validation.validator !== 'function'
    ) {
      return false;
    }
  }

  return true;
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