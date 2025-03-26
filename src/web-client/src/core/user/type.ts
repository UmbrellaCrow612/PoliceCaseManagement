/**
 * Represents a user obj
 */
export interface User {
  id: string;
  userName: string;
  email: string;
}
/**
 * Response from server shape dto for /me endpoint
 */
export interface FetchUserResponseBody {
  user: User;
  roles: string[];
}

export interface CreateUserResponseBody {
  id: string;
  username: string;
  email: string;
}
