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
