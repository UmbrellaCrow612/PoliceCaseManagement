import { AbstractControl, ValidationErrors } from '@angular/forms';
import { ExtractedUrls, ExtractionResult } from './types';

/**
 * Extracts a single http and https launch URL from the string content of a launchSettings.json file
 * using regular expressions. It returns a result object indicating success or failure.
 *
 * @param fileContent The raw string content of the launchSettings.json file.
 * @returns An `ExtractionResult` object. If successful, `success` is true and `data` contains the URLs.
 *          If no URLs are found, `success` is false and `reason` explains why.
 */
export function extractLaunchUrls(fileContent: string): ExtractionResult {
  // This object will hold the final URLs found.
  const extractedData: ExtractedUrls = {
    http: null,
    https: null,
  };

  // Regex to find all "applicationUrl" values.
  const appUrlRegex = /"applicationUrl"\s*:\s*"([^"]+)"/g;
  const matches = fileContent.matchAll(appUrlRegex);

  for (const match of matches) {
    // The captured group is the content inside the quotes (e.g., "https://...;http://...")
    const urlBlock = match[1];
    const urls = urlBlock.split(';');

    for (const url of urls) {
      const trimmedUrl = url.trim();

      if (trimmedUrl.startsWith('http://')) {
        // If multiple http URLs are found, the last one will be used.
        extractedData.http = trimmedUrl;
      } else if (trimmedUrl.startsWith('https://')) {
        // If multiple https URLs are found, the last one will be used.
        extractedData.https = trimmedUrl;
      }
    }
  }

  // After checking all matches, determine if the operation was successful.
  // Success is defined as finding at least one http or https URL.
  if (extractedData.http !== null || extractedData.https !== null) {
    return {
      success: true,
      data: extractedData,
    };
  } else {
    // If we looped through everything and found nothing, return a failure result.
    return {
      success: false,
      reason: 'No http or https application URLs were found in the file.',
    };
  }
}

export function validUrl(control: AbstractControl): ValidationErrors | null {
  try {
    new URL(control.value);
    return null;
  } catch {
    return { url: 'invalid URL', value: control.value };
  }
}
