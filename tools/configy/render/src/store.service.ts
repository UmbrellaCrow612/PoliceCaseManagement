import { Injectable } from '@angular/core';
import { ReadFileInfo } from './types';

@Injectable({
  providedIn: 'root',
})
export class StoreService {
  private _selectedDirectory: string | null = null;
  private _launchSettingFileInfo: ReadFileInfo[] = [];

  setSelectedDirectory(path: string) {
    this._selectedDirectory = path;
  }

  getSelectedDirectory() {
    return this._selectedDirectory;
  }

  setLaunchSettingFiles(filesInfo: ReadFileInfo[]) {
    this._launchSettingFileInfo = filesInfo;
  }

  getLaunchSettingFiles() {
    return this._launchSettingFileInfo;
  }
}
