import { Component } from '@angular/core';
import { OpenDialogReturnValue } from 'electron';
import { EWindow } from '../../../../types';

@Component({
  selector: 'app-start-view',
  imports: [],
  templateUrl: './start-view.component.html',
  styleUrl: './start-view.component.css',
})
export class StartViewComponent {
  /**
   * Folder picked
   */
  solutionFolder: string | null = null;

  /**
   * Holds any loading state
   */
  loading = false;

  /**
   * Holds any error state
   */
  error: string | null = null;

  /**
   * Handles loading and opening desktop solution folder and files
   */
  async loadSolutionFolder() {
    this.loading = true;
    this.error = null;
    this.solutionFolder = null;

    /**
     * Regular window obj but we will acess electron api
     */
    let _window = window as unknown as EWindow;

    if (_window.electronAPI.works()) {
      var res = await _window.electronAPI.openDirectory();

      if (res.canceled) {
        this.error = 'Cancel open folder try again';
        this.loading = false;
        return;
      } else {
        this.solutionFolder = res.filePaths[0];
        this.loading = false;
      }
    } else {
      this.error = 'Failed to open directory somthing is wrong';
    }
  }

  /**
   * Cancel a selected solution folder and resets sate
   */
  cancel(){
    this.error = null;
    this.solutionFolder = null;
  }
}
