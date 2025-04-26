
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
  { name: 'Pending Review', number: 1 },
  { name: 'Active', number: 2 },
  { name: 'Suspended', number: 3 },
  { name: 'Warrant Issued', number: 4 },
  { name: 'Referred', number: 5 },
  { name: 'Closed Cleared', number: 6 },
  { name: 'Closed Unfounded', number: 7 },
  { name: 'Closed Unsolved', number: 8 },
  { name: 'Archived', number: 9 },
  { name: 'Invalid', number: 10 }
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

