/**
 * Represents a extraction result when reading a launch setting json file and tring to get url's out of it
 */
export type LaunchSettingsUrlExtractionResult = {
  success: boolean;
  httpUrl: string;
  httpsUrl: string;
  errorMessage: string;
};

export type KTPVType =
  | 'string'
  | 'number'
  | 'boolean'
  | 'object'
  | 'array'
  | 'function'
  | 'undefined'
  | 'symbol'
  | 'bigint';

/**
 * Represents metadata about a JSON object's key, including its name, type,
 * parent path (predicate), and raw value.
 *
 * @example
 * For `{ "name": "john" }` the KTPV would be:
 * {
 *   key: "name",
 *   type: "string",
 *   predicate: "",
 *   value: "john"
 * }
 */
export type KTPV = {
  /**
   * The key name.
   * @example
   * `{ "name": "john" }` → key = "name"
   */
  key: string;

  /**
   * The data type of the value.
   * @example
   * `{ "age": 25 }` → type = "number"
   */
  type: KTPVType;

  /**
   * A comma-separated list of parent keys showing the nesting path.
   *
   * @example
   * ```
   * {
   *   "parentOne": {
   *     "parentTwo": { "name": "john" }
   *   }
   * }
   * ```
   * For the key `"name"`, predicate = "parentOne,parentTwo"
   */
  predicate: string;

  /**
   * The raw value of the key.
   * @example
   * `{ "name": "john" }` → value = "john"
   */
  value: any;
};

