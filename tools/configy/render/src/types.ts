import { OpenDialogReturnValue } from 'electron';

/**
 * Represents a file read from a dotnet project - mainly json files - make sure they match main js function reads it
 */
export interface DotNetFile {
  filePath: string;
  fileContent: string;
  fileName: string;
}

/**
 * Keep up to date with preload js api
 */
export interface ElectronAPI {
  works: () => boolean;
  openDirectory: () => Promise<OpenDialogReturnValue>;
  readDotNetFiles: (dir: string) => Promise<Array<DotNetFile>>;
}

/**
 * Window object extened with electron API provided in dir above preload JS
 */
export interface EWindow extends Window {
  /**
   * Extened window api from electron shell
   */
  electronAPI: ElectronAPI;
}

/**
 * The structure of the URL data when extraction is successful.
 */
export interface ExtractedUrls {
  http: string | null;
  https: string | null;
}

/**
 * Represents a successful extraction result.
 */
type SuccessResult = {
  success: true;
  data: ExtractedUrls;
};

/**
 * Represents a failed extraction result, with a reason for the failure.
 */
type FailureResult = {
  success: false;
  reason: string;
};

/**
 * The combined result type, which can be either a success or a failure.
 * This forces you to check the `success` property before accessing `data`.
 */
export type ExtractionResult = SuccessResult | FailureResult;
