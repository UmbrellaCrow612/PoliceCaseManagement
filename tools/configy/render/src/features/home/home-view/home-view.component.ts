import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { StoreService } from '../../../store.service';
import { EWindow } from '../../../ewindow';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-home-view',
  imports: [MatButtonModule],
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
    this.errorMessage = null;
    this.selectedDirectory = null;

    /**
     * Window object with electron api extened on the window object
     */
    let ewidnow = window as unknown as EWindow;

    if (!ewidnow.electronAPI.works()) {
      this.errorMessage = 'Electron API not working';
    }

    const result = await ewidnow.electronAPI.openDirectory();
    if (result.canceled) {
      this.errorMessage = 'Canceled directory selection';
      return;
    }

    this.selectedDirectory = result.filePaths[0];
    this.store.setSelectedDirectory(this.selectedDirectory);
  }

  viewLaunchSettings() {
    this.router.navigate(['launch-settings']);
  }
}
