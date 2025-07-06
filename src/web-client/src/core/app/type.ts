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

  /**
   * List of permissions needed
   */
  permissionsNeeded: string[];
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

/**
 * Generic base type similar to the backend generic PaginatedResult<T> which is used for any paginated result
 */
export interface PaginatedResult<T> {
  /**
   * A list of items
   */
  data: T[];

  /**
   * Meta data related to do with pagination
   */
  pagination: {
    /**
     * The current page number fetched
     */
    currentPage: number;

    /**
     * How many items where fetched for the given paghed result
     */
    pageSize: number;

    /**
     * Total amount of pages avaiable to fetch
     */
    totalPages: number;

    /**
     * Totoal amount of items
     */
    totalRecords: number;
  };

  /**
   * If there is a next page to fetch
   */
  hasNextPage: boolean;

  /**
   * If there is a previous page to fetch
   */
  hasPreviousPage: boolean;
}
