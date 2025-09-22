import { Component, inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { ReadFileInfo } from '../../../types';
import { MatButtonModule } from '@angular/material/button';
import {
  checkStringsInContent,
  extractLaunchSettingUrls,
  isValidUrl,
  replaceStrings,
} from '../../../util';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { StoreService } from '../../../store.service';
import { EWindow } from '../../../ewindow';
import { checkApi } from '../../../works';

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
  store = inject(StoreService);

  httpControl = new FormControl('', {
    validators: [Validators.required, isValidUrl],
  });
  httpsControl = new FormControl('', {
    validators: [Validators.required, isValidUrl],
  });

  isSaving = false;

  async ngOnInit() {
    if (this.item == null) {
      this.err = 'Failed to pass item';
      return;
    }
    await this.readFile();
  }

  /**
   * Reads the launchsettings.json file and sets up state
   */
  async readFile() {
    this.isLoading = true;
    this.err = null;

    let ewindow = window as unknown as EWindow;
    if (!checkApi()) {
      this.err = 'Electron API not working';
      this.isLoading = false;
    }

    var fileContent = await ewindow.electronAPI.readFile(this.item.filePath);
    if (fileContent === '') {
      this.err = 'File not found or is empty';
      this.isLoading = false;
      return;
    }

    var res = extractLaunchSettingUrls(fileContent);
    if (!res.success) {
      this.err = res.errorMessage ?? 'Could not find http or https URLS';
      this.isLoading = false;
      return;
    }
    this.httpControl.setValue(res.httpUrl);
    this.httpsControl.setValue(res.httpsUrl);

    this.isLoading = false;
  }

  /**
   * Checks if a launchsetting.json http or https url is taken by another profile
   * runs on save to check
   */
  async isLaunchUrlTakenByAnotherProfile(
    httpUrl: string,
    httpsUrl: string
  ): Promise<{ taken: boolean; errMess: string }> {
    let ewidnow = window as unknown as EWindow;

    if (!checkApi()) {
      return { taken: true, errMess: 'Electron api not wokring' };
    }

    var allLaunchSettings = await ewidnow.electronAPI.readFiles(
      this.store.getSelectedDirectory()!,
      {
        fileNames: new Set(['launchsettings.json']),
        fileExts: new Set(),
        ignoreDirs: new Set(),
        ignoreFileNames: new Set(),
      }
    );
    var filtered = allLaunchSettings.filter(
      (x) => x.filePath.toLowerCase() !== this.item.filePath.toLowerCase()
    );

    for (var file of filtered) {
      let fileContent = await ewidnow.electronAPI.readFile(file.filePath);
      var colission = checkStringsInContent(fileContent, httpUrl, httpsUrl);
      if (colission) {
        return {
          taken: true,
          errMess: 'HTTP or HTTP string taken by another profile use another',
        };
      }
    }

    return { taken: false, errMess: '' };
  }

  /**
   * Saves new http and https urls and every ref to them in other files
   */
  async save() {
    this.err = null;
    this.isSaving = true;

    let ewidnow = window as unknown as EWindow;
    if (!checkApi()) {
      this.err = 'Failed electron api';
      return;
    }

    if (this.httpControl.invalid || this.httpsControl.invalid) {
      this.err = 'Form invalid';
      this.isSaving = false;
      return;
    }

    const newHttpUrl = this.httpControl.value!;
    const newHttpsUrl = this.httpsControl.value!;

    var res = await this.isLaunchUrlTakenByAnotherProfile(
      newHttpUrl,
      newHttpsUrl
    );
    if (res.taken) {
      this.err = 'Another profile is using the HTTP or HTTPS urls';
      this.isSaving = false;
      return;
    }

    // change orginal launch settings
    var orginalLaunchSettingContent = await ewidnow.electronAPI.readFile(
      this.item.filePath
    );
    var oeginalUrls = extractLaunchSettingUrls(orginalLaunchSettingContent);
    if (!oeginalUrls.success) {
      this.err =
        oeginalUrls.errorMessage ?? 'Could not find http or https URLS';
      this.isSaving = false;
      return;
    }

    var newLaunchSettingsContent = replaceStrings(
      orginalLaunchSettingContent,
      oeginalUrls.httpUrl,
      oeginalUrls.httpsUrl,
      newHttpUrl,
      newHttpsUrl
    );
    await ewidnow.electronAPI.overWriteFile(
      this.item.filePath,
      newLaunchSettingsContent
    );

    // change another other file refs - only source code ones i.e just source code string replacement
    var allFiles = await ewidnow.electronAPI.readFiles(
      this.store.getSelectedDirectory()!,
      {
        fileExts: new Set([
          // C# source code
          '.cs',
          '.cshtml', // Razor pages
          '.razor', // Blazor components

          // JavaScript / TypeScript
          '.js',
          '.ts',
          '.jsx',
          '.tsx',

          // Angular / Web
          '.html', // Angular templates
          '.css',
          '.scss',
          '.less',
          '.json', // Configuration files that might contain references
        ]),
        fileNames: new Set(),
        ignoreDirs: new Set(),
        ignoreFileNames: new Set(),
      }
    );
    var filteredAllFiles = allFiles.filter(
      (x) => x.filePath.toLowerCase() !== this.item.filePath.toLowerCase()
    );

    for (const file of filteredAllFiles) {
      let fileContent = await ewidnow.electronAPI.readFile(file.filePath);
      var newContent = replaceStrings(
        fileContent,
        oeginalUrls.httpUrl,
        oeginalUrls.httpsUrl,
        newHttpUrl,
        newHttpsUrl
      );
      await ewidnow.electronAPI.overWriteFile(file.filePath, newContent);
    }

    this.isSaving = false;
  }
}
