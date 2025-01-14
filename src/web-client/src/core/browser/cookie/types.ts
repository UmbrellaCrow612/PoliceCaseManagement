/**
 * Interface defining all available cookie attributes and options.
 */
export interface CookieOptions {
  SameSite?: 'Lax' | 'None' | 'Strict';
  Secure?: boolean;
  HttpOnly?: boolean; // This is for documentation purposes, cannot be set client-side.
  Path?: string;
  Domain?: string;
  Expires?: string | Date;
  MaxAge?: number;
}