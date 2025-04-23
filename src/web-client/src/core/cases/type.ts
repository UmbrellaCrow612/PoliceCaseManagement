
export interface CreateCase {
  caseNumber: string;

  summary: string | null;

  description: string | null;

  incidentDateTime: Date;

  reportingOfficerId: string;
}


export interface Case {
  id: string
  caseNumber: string
  summary: string | null
  description: string | null
  incidentDateTime: Date
  reportedDateTime: Date
  status: number
  priority: number
}


/**
 * Array containing the backend enum mapped here
 */
export const CaseStatusNames: { name: string, number: number }[] = [
  { name: 'Reported', number: 0 },
  { name: 'PendingReview', number: 1 },
  { name: 'Active', number: 2 },
  { name: 'Suspended', number: 3 },
  { name: 'WarrantIssued', number: 4 },
  { name: 'Referred', number: 5 },
  { name: 'ClosedCleared', number: 6 },
  { name: 'ClosedUnfounded', number: 7 },
  { name: 'ClosedUnsolved', number: 8 },
  { name: 'Archived', number: 9 }
];


/**
 * Array contaning the backend enum mapped here
 */
export const CasePriorityNames: { name: string, number: number }[] = [
  { name: 'Low', number: 0 },
  { name: 'Normal', number: 1 },
  { name: 'High', number: 2 },
  { name: 'Urgent', number: 3 },
  { name: 'Critical', number: 4 }
];

