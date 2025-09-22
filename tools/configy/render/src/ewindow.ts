import { OpenDialogReturnValue } from 'electron';
import { ReadFileInfo, ReadFileOptions } from './types';

/**
 * Keep up to date with preload js api for the functions offered by electron ipcrender
 */
export interface ElectronAPI {
  works: () => boolean;
  openDirectory: () => Promise<OpenDialogReturnValue>;
  readFiles: (dir: string, options: ReadFileOptions) => Promise<ReadFileInfo[]>;
  readFile: (filePath: string) => Promise<string>;
  overWriteFile: (
    filePath: string,
    newContent: string
  ) => Promise<{ success: boolean; error?: string }>;
}

/*
 * Electron API extened on the window object
 */
export interface EWindow extends Window {
  /**
   * Exposes the electron API
   */
  electronAPI: ElectronAPI;
}
