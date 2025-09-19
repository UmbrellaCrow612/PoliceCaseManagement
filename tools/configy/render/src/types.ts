import { OpenDialogReturnValue } from 'electron';

/**
 * Keep up to date with preload js api
 */
export interface ElectronAPI {
  works: () => boolean;
  openDirectory: () => Promise<OpenDialogReturnValue>;
}

/**
 * Winbdowe object extened with electron API provided in dir above preload JS
 */
export interface EWindow extends Window {
  /**
   * Extened window api from electron shell
   */
  electronAPI: ElectronAPI;
}
