import {
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';
import { UrlExtractionResult } from './types';

export function isValidUrl(control: AbstractControl): ValidationErrors | null {
  try {
    const url = new URL(control.value);

    // Check if host contains "localhost"
    if (!url.hostname.includes('localhost')) {
      return { url: "URL must contain 'localhost'", value: control.value };
    }

    // Check if port is present
    if (!url.port || isNaN(Number(url.port))) {
      return {
        url: "URL must include a port number after 'localhost:'",
        value: control.value,
      };
    }

    return null;
  } catch {
    return { url: 'Invalid URL', value: control.value };
  }
}

export function extractUrlsFromLaunchSettings(
  content: string
): UrlExtractionResult {
  if (!content || content.trim().length === 0) {
    return {
      success: false,
      errorMessage: 'Input string is empty or null.',
    };
  }

  // Find all applicationUrl values
  const appUrlMatches = [
    ...content.matchAll(/"applicationUrl"\s*:\s*"([^"]+)"/g),
  ];

  let httpUrl: string | undefined;
  let httpsUrl: string | undefined;

  for (const match of appUrlMatches) {
    const urls = match[1].split(';');
    for (const url of urls) {
      if (url.startsWith('https://') && !httpsUrl) {
        httpsUrl = url;
      }
      if (url.startsWith('http://') && !httpUrl) {
        httpUrl = url;
      }
    }
  }

  if (!httpUrl && !httpsUrl) {
    return {
      success: false,
      errorMessage:
        'No valid HTTP or HTTPS URLs found in applicationUrl fields.',
    };
  }

  return {
    success: true,
    httpUrl,
    httpsUrl,
  };
}
