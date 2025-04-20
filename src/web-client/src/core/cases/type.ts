/**
 * Create dto
 */
export interface CreateCase {
  caseNumber: string | null;

  summary: string | null;

  description: string | null;

  incidentDateTime: Date;

  reportingOfficerId: string;
}

/**
 * Backend model
 */
export interface Case {}
