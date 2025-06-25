/**
 * Helper function to see if a string is a valid email address format
 * @param str - String input
 * @returns {boolean} - Boolean if it was a valid email or not
 */
export function isEmail(str: string): boolean {
  const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

  return emailRegex.test(str);
}
