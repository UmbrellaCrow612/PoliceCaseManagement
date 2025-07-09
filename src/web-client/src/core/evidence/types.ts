import { PaginatedResult } from './../app/type';
/**
 * Tag evidence model
 */
export interface Tag {
  id: string;
  name: string;
}

/**
 * Search result for fetching a paged searched tags
 */
export interface TagPagedResult extends PaginatedResult<Tag> {}

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
