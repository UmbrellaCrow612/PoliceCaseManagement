import { AbstractControl, ValidationErrors } from '@angular/forms';
import { LaunchSettingsUrlExtractionResult } from './types';

/**
 * Angular validator function to check if a URL is valid and meets localhost requirements
 * @param control - The AbstractControl containing the URL value to validate
 * @returns ValidationErrors object if validation fails, null if validation passes
 *
 * Validation rules:
 * - Must be a valid URL format
 * - Must contain 'localhost' in the hostname
 * - Must include a valid port number
 *
 */
export function isValidUrl(control: AbstractControl): ValidationErrors | null {
  try {
    const url = new URL(control.value);

    // Check if host contains "localhost"
    if (!url.hostname.includes('localhost')) {
      return { urlValid: "URL must contain 'localhost'", value: control.value };
    }

    // Check if port is present
    if (!url.port || isNaN(Number(url.port))) {
      return {
        urlValid: "URL must include a port number after 'localhost:'",
        value: control.value,
      };
    }

    return null;
  } catch {
    return { urlValid: 'Invalid URL', value: control.value };
  }
}

/**
 * Extract the http and https urls from the`launchsettings.json`
 * @param fileContent The content of the launchsetting.json file as string content
 * @returns Result object contaning the url's if successful
 */
export function extractLaunchSettingUrls(
  fileContent: string
): LaunchSettingsUrlExtractionResult {
  var res: LaunchSettingsUrlExtractionResult = {
    errorMessage: '',
    httpsUrl: '',
    httpUrl: '',
    success: false,
  };

  var httpUrl = extractHttpUrl(fileContent);
  if (httpUrl === null) {
    res.errorMessage =
      'HTTP URL could not be extracted from launchsettings.json';
    return res;
  }
  res.httpUrl = httpUrl;

  var httpsUrl = extractHttpsUrl(fileContent);
  if (httpsUrl === null) {
    res.errorMessage =
      'HTTPS URL could not be extracted from launchsettings.json';
    return res;
  }
  res.httpsUrl = httpsUrl;

  res.success = true;
  return res;
}

/**
 * Get launchsettings.json http url
 * @param jsonString json string
 * @returns Match or null
 */
function extractHttpUrl(jsonString: string): string | null {
  const regex =
    /"http"\s*:\s*\{(?:[^{}]|\{[^}]*\})*?"applicationUrl"\s*:\s*"([^"]*)"/s;
  const match = jsonString.match(regex);

  if (match && match[1]) {
    return match[1];
  }
  return null;
}

/**
 * Get launchsettings.json https url
 * @param jsonString json string
 * @returns HTTPS URL only or null
 */
function extractHttpsUrl(jsonString: string): string | null {
  const regex =
    /"https"\s*:\s*\{(?:[^{}]|\{[^}]*\})*?"applicationUrl"\s*:\s*"([^"]*)"/s;
  const match = jsonString.match(regex);

  if (match && match[1]) {
    // Split by semicolon and find the HTTPS URL
    const urls = match[1].split(';');
    const httpsUrl = urls.find((url) => url.trim().startsWith('https://'));
    return httpsUrl ? httpsUrl.trim() : null;
  }
  return null;
}

/**
 * Check if strings show up in another string
 * @param content String to check
 * @param string1 String to check
 * @param string2 String to check
 * @returns Bool if they show up
 */
export function checkStringsInContent(
  content: string,
  string1: string,
  string2: string
) {
  return content.includes(string1) || content.includes(string2);
}

/**
 * Replaces occurrences of two target strings in a given text with their respective replacements.
 *
 * @param {string} content - The original string to perform replacements on.
 * @param {string} stringOne - The first string to find in the content.
 * @param {string} stringTwo - The second string to find in the content.
 * @param {string} stringThree - The string to replace stringOne with.
 * @param {string} stringFour - The string to replace stringTwo with.
 * @returns {string} - The updated string after replacements.
 */
export function replaceStrings(
  content: string,
  stringOne: string,
  stringTwo: string,
  stringThree: string,
  stringFour: string
) {
  if (typeof content !== 'string') return content;

  let result = content;

  if (stringOne) {
    const regexOne = new RegExp(stringOne, 'g'); // Replace all occurrences
    result = result.replace(regexOne, stringThree);
  }

  if (stringTwo) {
    const regexTwo = new RegExp(stringTwo, 'g'); // Replace all occurrences
    result = result.replace(regexTwo, stringFour);
  }

  return result;
}
