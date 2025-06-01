import { PagedResult } from '../app/type';

export interface CreateCase {
  caseNumber: string;

  summary: string | null;

  description: string | null;

  incidentDateTime: Date;

  reportingOfficerId: string;
}

export interface Case {
  id: string;
  caseNumber: string;
  summary: string | null;
  description: string | null;
  incidentDateTime: Date;
  reportedDateTime: Date;
  status: number;
  priority: number;
  reportingOfficerUserName: string;
  reportingOfficerId: string;
  reportingOfficerEmail: string;
}

export interface CreateCaseAction {
  description: string;
  notes: string | null;
}

export interface CaseAction {
  id: string;
  description: string | null;
  notes: string | null;
  createdAt: Date;
  createdById: string;
  createdByEmail: string;
  createdByName: string;
}

export interface CaseAttachment {
  id: string;
  fileName: string;
  contentType: string;
  fileSize: number;
  uploadedAt: Date;
}

export interface CasePermission {
  /**
   * NOTE this is not the user's ID but the specific permission itself refer to userId field for that
   */
  id: string;
  canEdit: boolean;
  canAssign: boolean;
  caseId: string;
  userId: string;
  userName: string;
}

/**
 * Array containing the backend enum mapped here
 */
export const CaseStatusNames: { name: string; number: number }[] = [
  { name: 'Reported', number: 0 },
  { name: 'Pending Review', number: 1 },
  { name: 'Active', number: 2 },
  { name: 'Suspended', number: 3 },
  { name: 'Warrant Issued', number: 4 },
  { name: 'Referred', number: 5 },
  { name: 'Closed Cleared', number: 6 },
  { name: 'Closed Unfounded', number: 7 },
  { name: 'Closed Unsolved', number: 8 },
  { name: 'Archived', number: 9 },
];

/**
 * Array contaning the backend enum mapped here
 */
export const CasePriorityNames: { name: string; number: number }[] = [
  { name: 'Low', number: 0 },
  { name: 'Normal', number: 1 },
  { name: 'High', number: 2 },
  { name: 'Urgent', number: 3 },
  { name: 'Critical', number: 4 },
];

export interface CasePagedResult extends PagedResult<Case> {}

/**
 * Contains all the permissions for cases mapped from the model table names
 */
export const CasePermissionNames = {
  canEdit: 'CanEdit',
  canAssign: 'CanAssign',

  /**
   * Returns an array of all permission names
   */
  all(): string[] {
    return [this.canEdit, this.canAssign];
  },
};
