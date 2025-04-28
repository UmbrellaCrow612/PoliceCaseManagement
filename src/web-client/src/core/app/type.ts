/**
 * Represents a <a> tag link used ot navigate in the app - these could be hidden based on certain conditions
 * as such we don't hard code links but have a specification way of storing links
 * and rendering them
 */
export interface AppLink {
  /**
   * The visual text of the link
   */
  name: string;

  /**
   * Contains all roles that can see it
   */
  authorizedRoles: string[];

  /**
   * The actual href to navigate to said resource view
   */
  href: string;
}

/**
 * Generic pagination result sent from backend
 */
export interface PagedResult<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}
