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
  | 'bigint'
  | 'boolean'
  | 'symbol'
  | 'undefined'
  | 'object'
  | 'function'
  | 'array';

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

export type KTPVDriftConfig = {
  /**
   * Pass this to check if a ktpv is a string and check if the corosponding key value contains a string
   */
  contains?: string;

  /**
   * Pass this to check if a ktpv is a number and check if a value number is greater than the given number
   */
  greaterThan?: number;

  /**
   * Pass this to check if a ktpv is a number and check if a value number is less than the given number
   */
  lessThan?: number;

  /**
   * If it should check the exact match
   */
  exactMatch?: boolean
};

/**
 * Gives a ktpv drift option for checking fort drift
 */
export type KTPVAndConfig = {
  ktpv: KTPV;
  config: KTPVDriftConfig;
};
