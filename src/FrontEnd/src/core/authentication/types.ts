/**
 * Represents the standard login credentials needed to authenticate a user
 */
export interface LoginCredentials {
  email: string | null | undefined;
  password: string | null | undefined;
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

/**
 * Represents the standard response you receive after a successful login
 */
export interface LoginResponse {
  id: string;
}

export interface SmsCodeRequest {
  loginAttemptId: string;
  code: string | null | undefined;
}

export interface SmsCodeResponse {
  code: string;
}
