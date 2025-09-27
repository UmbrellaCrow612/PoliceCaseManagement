/**
 * Represents a extraction result when reading a launch setting json file and tring to get url's out of it
 */
export type LaunchSettingsUrlExtractionResult = {
  success: boolean;
  httpUrl: string;
  httpsUrl: string;
  errorMessage: string;
};
