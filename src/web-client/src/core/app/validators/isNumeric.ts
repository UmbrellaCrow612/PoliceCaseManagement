/**
 * Checks if the given string contains only numeric digits.
 *
 * @param {string} str - The string to test.
 * @returns {boolean} Returns true if the string contains only numeric digits, otherwise false.
 */
export function isNumeric(str: string): boolean {
  return /^\d+$/.test(str);
}
