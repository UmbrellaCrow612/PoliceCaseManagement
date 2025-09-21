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
}

/*
 * Electron API extened on the window object
 */
export interface EWindow extends Window {
  electronAPI: ElectronAPI;
}

/**
 * Object to sotre generic way of information about a file that was read
 */
export interface ReadFileInfo {
  fileName: string;
  filePath: string;
  fileDirectory: string;
}
