/**
 * Object to store generic way of information about a file that was read from system
 */
export interface ReadFileInfo {
  fileName: string;
  filePath: string;
  fileDirectory: string;
}

/**
 * Represents a extraction result when reading a launch setting json file and tring to get url's out of it
 */
export type LaunchSettingsUrlExtractionResult = {
  success: boolean;
  httpUrl: string;
  httpsUrl: string;
  errorMessage: string;
};

/**
 * List of options you can pass when reading files from a directory
 */
export interface ReadFileOptions {
  /** List of file names to look for */
  fileNames: Set<string>;
  
  /** List of file extensions you want to look for */
  fileExts: Set<string>;
  
  /** List of file names to ignore */
  ignoreFileNames: Set<string>;
  
  /** List of folders to ignore */
  ignoreDirs: Set<string>;
}
