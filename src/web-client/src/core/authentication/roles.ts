/**
 * Backend roles mapped
 * NOTE:
 * Make sure there case sensitive for example if the backend name value of ADMIN is Admin make sure front is is ADMIN string value
 * if the front end is lowercase admin it will fail role check !
 */
export const UserRoles = {
  Admin: 'Admin',

  /**
   * Get All roles in the system
   * @returns List of all the system roles mapped from the backend
   */
  all() {
    return [this.Admin];
  },
} as const;
