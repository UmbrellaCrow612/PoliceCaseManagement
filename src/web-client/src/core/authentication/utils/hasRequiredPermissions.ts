/**
 * Checks if all required permissions are present in the current permissions.
 *
 * @param {string[]} requiredPermissionNames - An array of permission names that are required.
 * @param {string[]} currentPermissionNames - An array of permission names currently held by the user.
 * @returns {boolean} True if all required permissions are present or if no required permissions are specified; otherwise, false.
 */
export function hasRequiredPermissions(
  requiredPermissionNames: string[],
  currentPermissionNames: string[]
): boolean {
  if (requiredPermissionNames.length === 0) return true;

  return requiredPermissionNames.every((p) =>
    currentPermissionNames.includes(p)
  );
}
