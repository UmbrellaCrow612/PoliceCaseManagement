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



export interface CaseUser {
  /**
   * NOTE: this is not the user ID but ID of the link between the case and user ID refer to user ID
   */
  id:string

  /**
   * The given case they are linked to
   */
  caseId:string


  userEmail:string
  userId:string
  userName:string
}