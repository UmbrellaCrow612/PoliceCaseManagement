import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class StoreService {
  private _selectedDirectory: string | null = null;

  setSelectedDirectory(path: string) {
    this._selectedDirectory = path;
  }

  getSelectedDirectory() {
    return this._selectedDirectory;
  }
}
