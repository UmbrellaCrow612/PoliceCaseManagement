import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { StoreService } from '../../../store.service';
import { EWindow } from '../../../ewindow';

@Component({
  selector: 'app-home-view',
  imports: [],
  templateUrl: './home-view.component.html',
  styleUrl: './home-view.component.css',
})
export class HomeViewComponent {
  router = inject(Router);
  store = inject(StoreService);

  /**
   * Selected directory
   */
  selectedDirectory: string | null = null;

  /**
   * Holds error message
   */
  errorMessage: string | null = null;

  /**
   * Selects a directory
   */
  async selectDirectory() {
    /**
     * Window object with electron api extened on the window object
     */
    let ewidnow = window as unknown as EWindow;

    if (ewidnow.electronAPI.works()) {
      const result = await ewidnow.electronAPI.openDirectory();
      if (result.canceled) {
        this.errorMessage = 'Canceled directory selection';
        return;
      }

      this.selectedDirectory = result.filePaths[0];
      this.store.setSelectedDirectory(this.selectedDirectory);
    } else {
      this.errorMessage = 'Electron API not working';
    }
  }

  viewLaunchSettings() {
    this.router.navigate(['launch-settings']);
  }
}
