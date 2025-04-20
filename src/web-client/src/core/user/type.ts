/**
 * Represents a user obj
 */
export interface User {
  id: string;
  userName: string;
  email: string;
  phoneNumber: string;
}

/**
 * Dto for uer details that are restricted mapped from backend
 */
export interface RestrictedUser {
  id: string;
  userName: string;
}
