/**
 * Backend Dto for person object
 */
export interface Person {
  id: string;
  firstName: string;
  lastName: string;
  dateOfBirth: Date;
  createdAt: Date;
  updatedAt: Date;
  phoneNumber: string;
  email: string;
}

/**
 * Represents the available search parameters for querying a person in the system.
 *
 * Each field corresponds to a backend query parameter. For example, the `firstName` field
 * will be appended to the URL as `/search?firstName=value`.
 *
 * Fields with a `null` value will be excluded from the final query string.
 * Ensure the field names match the expected parameter names used by the backend API.
 */
export interface SearchPersonQuery {
  firstName: string | null;
  lastName: string | null;
  dateOfBirth: Date | null;
  phoneNumber: string | null;
  email: string | null;
  pageNumber: number;
  pageSize: number;
}

/**
 * Fields sent across to create a person
 */
export interface CreatePersonDto {
  firstName: string;
  lastName: string;
  dateOfBirth: Date;
  phoneNumber: string;
  email: string;
}

/**
 * Backend dto mapped here for case person
 */
export interface CasePerson {
  Id: string;
  CaseId: string;
  Role: number;
  PersonId: string;
  PersonFirstName: string;
  PersonLastName: string;
  PersonDateBirth: Date;
  PersonPhoneNumber: string;
  PersonEmail: string;
}

/**
 * Backend enum mapped here for CasePersonRole
 */
export const CasePersonRoleNames: { name: string; value: number }[] = [
  {
    name: 'Suspect',
    value: 0,
  },
  {
    name: 'Person Of Interest',
    value: 1,
  },
  {
    name: 'Witness',
    value: 2,
  },
  {
    name: 'Complainant',
    value: 3,
  },
  {
    name: 'Victim',
    value: 4,
  },
];
