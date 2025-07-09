import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

/**
 * A mapping of MIME types to their maximum allowed file size in bytes.
 * Example:
 * {
 *   'audio/mpeg': 10485760, // 10 MB
 *   'application/pdf': 5242880 // 5 MB
 * }
 */
const appEvidenceUploadFileSizeLimits: Record<string, number> = {
  'audio/mpeg': 10 * 1024 * 1024, // 10 MB for MP3
  'application/pdf': 5 * 1024 * 1024, // 5 MB for PDFs
  'image/jpeg': 3 * 1024 * 1024, // 3 MB for JPEG images
};

/**
 * The default maximum file size in bytes if no MIME type-specific limit is configured.
 */
const defaultMaxFileSizeBytes = 2 * 1024 * 1024; // 2 MB

/**
 * Creates a synchronous validator function to validate the maximum file size
 * of an uploaded file based on its MIME type.
 *
 * If the MIME type is found in the `appEvidenceUploadFileSizeLimits` configuration,
 * that limit is used. Otherwise, the `defaultMaxFileSizeBytes` is applied.
 *
 * @returns {ValidatorFn} A validator function that returns a validation error object
 * if the file exceeds the allowed size, or `null` if the file is valid or not set.
 */
export function evidenceMaxFileSizeSyncValidator(): ValidatorFn {
  /**
   * The validator function that performs the file size validation.
   *
   * @param {AbstractControl<File | null>} control - The form control containing the file.
   * @returns {ValidationErrors | null} Validation errors if invalid, or null if valid.
   */
  return (control: AbstractControl<File | null>): ValidationErrors | null => {
    const file = control.value;

    if (!file || !(file instanceof File)) {
      return null;
    }

    const mimeType = file.type;
    const maxSize =
      appEvidenceUploadFileSizeLimits[mimeType] ?? defaultMaxFileSizeBytes;

    const size = file.size;

    if (size > maxSize) {
      return {
        maxFileSize: {
          /** The maximum allowed size in bytes */
          requiredMaxSize: maxSize,
          /** The actual size of the uploaded file in bytes */
          actualSize: size,
          /** The MIME type of the file */
          mimeType: mimeType || 'unknown',
        },
      };
    }

    return null;
  };
}
