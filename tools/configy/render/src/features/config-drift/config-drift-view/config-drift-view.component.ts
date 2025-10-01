import { Component, inject, OnInit } from '@angular/core';
import { StoreService } from '../../../store.service';
import { checkApi } from '../../../works';
import { getApi } from '../../../api';
import { diffKTPVs, getKTPV } from '../../../util';
import { KTPV } from '../../../types';
import { Router } from '@angular/router';
import { JsonPipe } from '@angular/common';

@Component({
  selector: 'app-config-drift-view',
  imports: [],
  templateUrl: './config-drift-view.component.html',
  styleUrl: './config-drift-view.component.css',
})
export class ConfigDriftViewComponent implements OnInit {
  ngOnInit(): void {
    if (!this.store.getSelectedDirectory()) {
      this.router.navigate(['/']);
    }
  }
  router = inject(Router);
  store = inject(StoreService);
  err: string | null = null;
  isLoading = false;

  sourceDriftFilePath: string | null = null;

  /**
   * Presenets a object with a ktp and it's options for drift matching - of the source file
   */
  ktpvs: Array<KTPV> = [];

  isScaning = false;

  driftFiles: Array<{ file: ReadFileInfo; diffKtpvs: Array<KTPV> }> = [];

  /**
   * Select the source file to to use for scanning for config drift
   */
  async selectSourceFile() {
    this.err = null;
    this.isLoading = true;
    this.ktpvs = [];
    this.sourceDriftFilePath = null;
    this.driftFiles = [];

    if (!(await checkApi())) {
      this.err = 'Electron api failed';
      this.isLoading = false;
      return;
    }
    let api = getApi();

    var res = await api.openFile({
      fileFil: [{ extensions: ['json'], name: 'JSON' }],
    });
    if (res.canceled) {
      this.err = 'Canceled file select';
      this.isLoading = false;
      return;
    }

    this.sourceDriftFilePath = res.filePaths[0];
    let content = await api.readFile(this.sourceDriftFilePath);

    try {
      let jsonParse = JSON.parse(content);
      this.ktpvs = getKTPV(jsonParse);
      this.isLoading = false;
    } catch (error) {
      this.err = error as string;
      this.isLoading = false;
    }
  }

  /**
   * Scans all other json files in the selected dir and displays any which are missing a key value pair from the org file and which keys
   */
  async scanConfigDrift() {
    this.err = null;
    this.isScaning = true;
    this.driftFiles = [];

    if (!(await checkApi())) {
      this.err = 'Electron api failed';
      this.isScaning = false;
      return;
    }

    let api = getApi();

    var jsonFilesInfo = await api.readFiles(
      this.store.getSelectedDirectory()!,
      {
        fileExts: new Set(['.json']),
        fileNames: new Set(),
        ignoreDirs: new Set(),
        ignoreFileNames: new Set(),
      }
    );

    var filteredJson = jsonFilesInfo.filter(
      (x) => x.filePath != this.sourceDriftFilePath
    );

    for (var item of filteredJson) {
      let fileContent = await api.readFile(item.filePath);

      try {
        let parsed = JSON.parse(fileContent);
        let itemsKTPVS = getKTPV(parsed);

        let diff = diffKTPVs(this.ktpvs!, itemsKTPVS);
        if (diff.length > 0) {
          this.driftFiles.push({ diffKtpvs: diff, file: item });
        }
      } catch (error) {}
    }
  }
}
