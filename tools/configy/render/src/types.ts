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
