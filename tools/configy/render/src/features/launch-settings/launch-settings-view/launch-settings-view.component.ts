import { Component, inject, OnInit } from '@angular/core';
import { StoreService } from '../../../store.service';
import { Router } from '@angular/router';
import { EWindow, ReadFileInfo } from '../../../types';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { LaunchSettingsDialogComponent } from '../launch-settings-dialog/launch-settings-dialog.component';

@Component({
  selector: 'app-launch-settings-view',
  imports: [MatButtonModule],
  templateUrl: './launch-settings-view.component.html',
  styleUrl: './launch-settings-view.component.css',
})
export class LaunchSettingsViewComponent implements OnInit {
  store = inject(StoreService);
  router = inject(Router);
  readonly dialog = inject(MatDialog);

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
    } else {
      this.errorMessage = 'Electron API failed';
    }
  }

  /**
   * Runs whe tryinmg to view a specific launch settings item
   */
  viewLaunchSetting(item: ReadFileInfo) {
    console.log('Giving dialog ' + item.filePath);
    this.dialog.open(LaunchSettingsDialogComponent, {
      data: {
        item: item,
      },
    });
  }
}
