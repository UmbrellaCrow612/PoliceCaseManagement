import { KTPV, KTPVAndConfig } from '../../types';

/**
 * Recursively get all keys of an object along with their types and predicate path (parent keys only) and value
 * @param {Object} obj The object to get keys from
 * @returns {Array<KTPV>}
 *   An array of objects of ktpv (Key, type, predicate, value)
 */
export function getKTPVs(obj: Object): Array<KTPV> {
  const result: Array<KTPV> = [];

  function helper(current: any, parentPredicate: string) {
    if (current && typeof current === 'object') {
      for (const key of Object.keys(current)) {
        const value = current[key];
        const type = Array.isArray(value) ? 'array' : typeof value;

        // Use parentPredicate as-is, do not include current key
        result.push({
          key: key,
          type: type,
          predicate: parentPredicate,
          value: value,
        });

        if (type === 'object') {
          // Pass updated predicate including current key to children
          const newPredicate = parentPredicate
            ? `${parentPredicate},${key}`
            : key;
          helper(value, newPredicate);
        }
      }
    }
  }

  helper(obj, ''); // start with empty predicate
  return result;
}

/**
 * Deep check if a obj's are equal
 * @param a Obj a
 * @param b Obj b
 * @returns True or false
 */
function isEqual(a: any, b: any): boolean {
  if (a === b) return true;

  if (Array.isArray(a) && Array.isArray(b)) {
    if (a.length !== b.length) return false;
    return a.every((v, i) => isEqual(v, b[i]));
  }

  if (a && b && typeof a === 'object' && typeof b === 'object') {
    const aKeys = Object.keys(a);
    const bKeys = Object.keys(b);
    if (aKeys.length !== bKeys.length) return false;
    return aKeys.every((k) => isEqual(a[k], b[k]));
  }

  return false;
}

/**
 * Get a list of missing ktpv from source for a given ktpv
 * @param first The source ktpv with there filter config options
 * @param second The ktpvs to check agaisnt
 * @returns {Array<KTPV>} List of missing KTPV from the source
 */
export function diffKTPVs(
  source: Array<KTPVAndConfig>,
  against: Array<KTPV>
): Array<KTPV> {
  const missing: Array<KTPV> = [];

  source.forEach((el) => {
    const found = against.find((x) => {
      // Match key and predicate first
      if (x.key !== el.ktpv.key || x.predicate !== el.ktpv.predicate)
        return false;

      // Handle 'string' type with 'contains' config
      if (
        el.config.contains &&
        el.config.contains.trim() !== '' &&
        x.type === 'string'
      ) {
        return (
          typeof x.value === 'string' && x.value.includes(el.config.contains)
        );
      }

      // Handle 'number' type with greaterThan
      if (
        el.config.greaterThan &&
        el.config.greaterThan !== undefined &&
        el.config.greaterThan !== null &&
        x.type === 'number'
      ) {
        return typeof x.value === 'number' && x.value > el.config.greaterThan;
      }

      // Handle 'number' type with lessThan
      if (
        el.config.lessThan &&
        el.config.lessThan !== undefined &&
        el.config.lessThan !== null &&
        x.type === 'number'
      ) {
        return typeof x.value === 'number' && x.value < el.config.lessThan;
      }

      if (
        el.config.exactMatch !== undefined &&
        el.config.exactMatch !== null &&
        typeof el.config.exactMatch === 'boolean' &&
        el.config.exactMatch
      ) {
        return isEqual(x.value, el.ktpv.value);
      }

      return true;
    });

    if (!found) {
      missing.push(el.ktpv);
    }
  });

  return missing;
}
