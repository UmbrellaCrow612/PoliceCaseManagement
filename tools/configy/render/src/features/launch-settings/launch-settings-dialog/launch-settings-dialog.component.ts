import { Component, inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { EWindow, ReadFileInfo } from '../../../types';
import { MatButtonModule } from '@angular/material/button';
import { extractUrlsFromLaunchSettings, isValudUrl } from '../../../util';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-launch-settings-dialog',
  imports: [
    MatDialogModule,
    MatButtonModule,
    MatInputModule,
    MatFormFieldModule,
    ReactiveFormsModule,
  ],
  templateUrl: './launch-settings-dialog.component.html',
  styleUrl: './launch-settings-dialog.component.css',
})
export class LaunchSettingsDialogComponent implements OnInit {
  item: ReadFileInfo = inject(MAT_DIALOG_DATA).item;
  err: string | null = null;
  isLoading = false;

  httpControl = new FormControl('', [Validators.required, isValudUrl]);
  httpsControl = new FormControl('', [Validators.required, isValudUrl]);

  async ngOnInit() {
    if (this.item == null) {
      this.err = 'Failed to pass item';
      return;
    }

    await this.readFile();
  }

  async readFile() {
    this.isLoading = true;
    this.err = null;

    let ewindow = window as unknown as EWindow;
    if (ewindow.electronAPI.works()) {
      var fileContent = await ewindow.electronAPI.readFile(this.item.filePath);
      if (fileContent === '') {
        this.err = 'File not found or is empty';
        this.isLoading = false;
        return;
      }

      var res = extractUrlsFromLaunchSettings(fileContent);
      if (!res.success) {
        this.err = res.errorMessage ?? 'Could not find http or https URLS';
        this.isLoading = false;
        return;
      }

      this.httpControl.setValue(res.httpUrl!);
      this.httpsControl.setValue(res.httpsUrl!);
      this.isLoading = false;
    } else {
      this.err = 'Electron API not working';
      this.isLoading = false;
    }
  }
}
