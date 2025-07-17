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

export interface CasePagedResult extends PagedResult<Case> {}

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
] as const;

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

/**
 * The backend enum for CaseRole mapped over here
 */
export const CaseRoleNames = {
  /**
   * The person who created the case
   */
  Owner: 0,

  /**
   * A person who can view and edit some aspects of the case
   */
  Editor: 1,

  /**
   * A person who can only view details
   */
  Viewer: 2,
} as const;

// Type: 'Owner' | 'Editor' | 'Viewer'
/**
 * String name value of CaseRoleNames
 */
export type CaseRoleName = keyof typeof CaseRoleNames;

// Type: 0 | 1 | 2
/**
 * Values of CaseRoleNames which is a number
 */
export type CaseRoleValue = (typeof CaseRoleNames)[CaseRoleName];

/**
 * CaseRoleNames Map to be used with index access
 */
export const CaseRoleNameMap = Object.fromEntries(
  Object.entries(CaseRoleNames).map(([key, value]) => [value, key])
) as Record<CaseRoleValue, CaseRoleName>;
