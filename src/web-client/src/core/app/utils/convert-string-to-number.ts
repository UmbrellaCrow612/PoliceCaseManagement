/**
 * Attempts to convert a string to a number.
 * Returns the number if successful, otherwise returns null.
 *
 * @param {string} value - The string to convert to a number.
 * @returns {number | null} The converted number or null if conversion fails.
 */
export function tryConvertStringToNumber(value: string): number | null {
    const num = Number(value);
    return isNaN(num) ? null : num;
  }
  