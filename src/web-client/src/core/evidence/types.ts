import { PaginatedResult } from './../app/type';
/**
 * Tag evidence model
 */
export interface Tag {
  id: string;
  name: string;
}

/**
 * What we need such as meta data to upload a piece of evidence
 */
export interface UploadEvidence {
  fileName: string;
  description: string | null;
  referenceNumber: string;
  collectionDate: Date;
  fileSize: number;
  contentType: string;
}

/**
 * The backend enum for ordering evidence
 */
export const EvidenceOrderByNames: { value: number; name: string }[] = [
  {
    value: 0,
    name: 'Collection Date',
  },
  {
    value: 1,
    name: 'Uploaded At',
  },
] as const;

/**
 * Represents the backend enum for file uploaded status for evidence
 */
export const EvidenceFileUploadStatusNames: { value: number; name: string }[] =
  [];

/**
 * Contaisn all the ways to search evidence in the system - mapped from backend query object for searching evidence
 */
export interface SearchEvidenceQuery {
  referenceNumber: string | null;
  fileName: string | null;
  contentType: string | null;
  uploadedAt: Date | null;
  collectionDate: Date | null;
  pageNumber: number;
  pageSize: number;
  orderBy: number | null;
  uploadedById: string | null;
}

/**
 * Represents a piece of evidence - backend Dto mapped here
 */
export interface Evidence {
  id: string;
  collectionDate: Date;
  contentType: string;
  description: string;
  fileName: string;
  fileSize: number;
  fileUploadStatus: number;
  referenceNumber: string;
  uploadedAt: Date;
  uploadedByEmail: string;
  uploadedById: string;
  uploadedByUsername: string;
}

/**
 * Represents the link between a case and a piece of evidence DTO mapped here
 */
export interface CaseEvidence {
  id: string;
  caseId: string;
  evidenceId: string;
  evidenceName: string;
  evidenceReferenceNumber: string;
}
