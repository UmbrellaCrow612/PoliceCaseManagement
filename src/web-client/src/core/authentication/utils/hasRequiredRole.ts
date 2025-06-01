/**
 * Checks if a user has at least one of the required roles.
 * If `requiredRoles` is an empty array, it means no specific roles are required, so the function returns `true`.
 *
 * @param {string[]} requiredRoles - Array of required roles.
 * @param {string[]} userRoles - Array of roles the user has.
 * @returns {boolean} `true` if the user has at least one of the required roles or if `requiredRoles` is empty, otherwise `false`.
 */
export function hasRequiredRole(
  requiredRoles: string[],
  userRoles: string[]
): boolean {
  if (requiredRoles.length === 0) {
    return true;
  }
  return requiredRoles.some((role) => userRoles.includes(role));
}
