import { Component, inject, OnInit } from '@angular/core';
import { StoreService } from '../../../store.service';
import { Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { LaunchSettingsDialogComponent } from '../launch-settings-dialog/launch-settings-dialog.component';
import { checkApi } from '../../../works';
import { getApi } from '../../../api';

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
    if (!checkApi()) {
      this.errorMessage = 'Electron API failed';

      return;
    }

    let api = getApi();

    const launchFiles = await api.readFiles(this.selectedDir!, {
      ignoreDirs: new Set(),
      fileExts: new Set(),
      fileNames: new Set(['launchsettings.json']),
      ignoreFileNames: new Set(),
    });

    this.launchSettingsFiles = launchFiles;
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
