import { OpenDialogReturnValue } from 'electron';

/**
 * Keep up to date with preload js api for the functions offered by electron ipcrender
 */
export interface ElectronAPI {
  works: () => boolean;
  openDirectory: () => Promise<OpenDialogReturnValue>;
  readFiles: (
    dir: string,
    extensions: Array<string>
  ) => Promise<ReadFileInfo[]>;
  readFile: (filePath: string) => Promise<string>;
}

/*
 * Electron API extened on the window object
 */
export interface EWindow extends Window {
  electronAPI: ElectronAPI;
}

/**
 * Object to store generic way of information about a file that was read from system
 */
export interface ReadFileInfo {
  fileName: string;
  filePath: string;
  fileDirectory: string;
}
