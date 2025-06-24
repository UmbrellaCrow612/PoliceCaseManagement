import { Pipe, PipeTransform } from '@angular/core';

/**
 * Optional parameters for the FileBytePipe.
 */
export interface FileBytePipeOptions {
  /**
   * The number of decimal places to include in the output.
   * @default 2
   */
  precision?: number;
}

const UNITS = [
  { unit: 'TB', value: 1e12 }, // Terabyte
  { unit: 'GB', value: 1e9 },  // Gigabyte
  { unit: 'MB', value: 1e6 },  // Megabyte
  { unit: 'KB', value: 1e3 },  // Kilobyte
  { unit: 'B',  value: 1 },    // Byte
];

/**
 * Angular Pipe to convert a raw byte value into the most appropriate,
 * human-readable format (e.g., KB, MB, GB).
 *
 * Usage examples in template:
 * ```html
 * <!-- Automatically determines the best unit -->
 * {{ 15360 | fileBytePipe }}               <!-- Outputs: "15.36 KB" -->
 * {{ 2500000 | fileBytePipe }}             <!-- Outputs: "2.50 MB" -->
 *
 * <!-- Optionally specify precision -->
 * {{ 15360 | fileBytePipe:{ precision: 0 } }}  <!-- Outputs: "15 KB" -->
 * ```
 *
 * Note: This pipe uses decimal (base-10) calculations where 1 KB = 1,000 bytes.
 */
@Pipe({
  name: 'fileBytePipe',
})
export class FileBytePipe implements PipeTransform {
  /**
   * Transforms a byte value into a human-readable string with the best-fitting unit.
   *
   * @param value The numeric byte value to convert.
   * @param options Optional parameters, e.g., for setting decimal precision.
   * @returns A formatted string representation (e.g., "10.24 KB", "2.50 MB").
   */
  transform(
    value: number | null | undefined,
    options: FileBytePipeOptions = {}
  ): string {
    if (value == null || isNaN(value)) {
      return 'N/A';
    }

    if (value < 0) {
      return 'Invalid size';
    }

    if (value === 0) {
      return '0 B';
    }

    const precision = options.precision === 0 ? 0 : options.precision || 2;

    const foundUnit = UNITS.find(unit => value >= unit.value);

    if (!foundUnit) {
      return `${value} B`;
    }

    const formattedValue = value / foundUnit.value;

    if (foundUnit.unit === 'B') {
        return `${Math.floor(formattedValue)} B`;
    }

    const fixedValue = formattedValue.toFixed(precision);
    return `${parseFloat(fixedValue)} ${foundUnit.unit}`;
  }
}