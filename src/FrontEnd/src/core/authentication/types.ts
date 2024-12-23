/**
 * Represents the standard login credentials needed to authenticate a user
 */
export interface LoginCredentials {
    userName: string | null,
    email: string | null,
    password: string
}

/**
 * Represents the standard JWT payload structure with common claims
 * Extend this interface to add custom claims specific to your application
 */
export interface JwtPayload {
  /** Issued at timestamp */
  iat?: number;
  /** Expiration timestamp */
  exp?: number;
  /** Not before timestamp */
  nbf?: number;
  /** JWT ID */
  jti?: string;
  /** Issuer */
  iss?: string;
  /** Subject (usually user ID) */
  sub?: string;
  /** Audience */
  aud?: string | string[];
  /** User email (common custom claim) */
  email?: string;
  /** User roles/permissions (common custom claim) */
  roles?: string[];
}


export interface JwtResponse {
  accessToken: string;
  refreshToken : string;
}