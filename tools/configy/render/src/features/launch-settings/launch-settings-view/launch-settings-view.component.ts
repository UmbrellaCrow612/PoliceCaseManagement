import { Component, inject, OnInit } from '@angular/core';
import { StoreService } from '../../../store.service';
import { Router } from '@angular/router';
import { EWindow, ReadFileInfo } from '../../../types';
import { JsonPipe } from '@angular/common';

@Component({
  selector: 'app-launch-settings-view',
  imports: [JsonPipe],
  templateUrl: './launch-settings-view.component.html',
  styleUrl: './launch-settings-view.component.css',
})
export class LaunchSettingsViewComponent implements OnInit {
  store = inject(StoreService);
  router = inject(Router);

  selectedDir: string = '';

  errorMessage: string | null = null;

  launchSettingsFiles: ReadFileInfo[] = [];

  ngOnInit(): void {
    if (this.store.getSelectedDirectory() === null) {
      this.router.navigate(['/']);
    }
    this.selectedDir = this.store.getSelectedDirectory()!;
  }

  /**
   * Scans a dir and gets all launch setting files
   */
  async scanLaunchSettingsFiles() {
    // Assuming 'EWindow' is a custom type you have defined
    let ewindow = window as unknown as EWindow;

    if (ewindow.electronAPI.works()) {
      const allJsonFiles = await ewindow.electronAPI.readFiles(
        this.selectedDir!,
        ['.json']
      );

      const launchSettingsFiles = allJsonFiles.filter(
        (file) => file.fileName.toLowerCase() === 'launchsettings.json'
      );
      this.launchSettingsFiles = launchSettingsFiles;

      console.log(launchSettingsFiles);

      this.store.setLaunchSettingFiles(launchSettingsFiles);
    } else {
      this.errorMessage = 'Electron API failed';
    }
  }
}
