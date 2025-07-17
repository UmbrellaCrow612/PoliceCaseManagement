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

/**
 * Details of a specific user linked to a specific case on there access list - contaisn there info and role on it
 */
export interface CaseAcessList {
  id: string;
  /**
   * The given case they are linked to
   */
  caseId: string;

  userEmail: string;
  userId: string;
  userName: string;

  caseRole: number;
}
