import { Component, inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { ReadFileInfo } from '../../../types';
import { MatButtonModule } from '@angular/material/button';
import { extractUrlsFromLaunchSettings, isValidUrl } from '../../../util';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { StoreService } from '../../../store.service';
import { from, map, mergeMap, Observable, of, switchMap, toArray } from 'rxjs';
import { EWindow } from '../../../ewindow';

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
    updateOn: 'blur',
  });
  httpsControl = new FormControl('', {
    validators: [Validators.required, isValidUrl],
    updateOn: 'blur',
  });

  isSaving = false;

  async ngOnInit() {
    if (this.item == null) {
      this.err = 'Failed to pass item';
      return;
    }

    this.httpControl.valueChanges.subscribe((val) => {
      this.IsUrlTakenByAnother(val!, this.item.filePath).subscribe((val) => {
        if (val) {
          console.log('taken by another');
          this.httpControl.setErrors(val);
        }
      });
    });

    this.httpsControl.valueChanges.subscribe((val) => {
      this.IsUrlTakenByAnother(val!, this.item.filePath).subscribe((val) => {
        if (val) {
          console.log('taken by another');
          this.httpsControl.setErrors(val);
        }
      });
    });

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

  /**
   * Check if a contorl value url is taken by another launch profile
   * @param controlValue Http or https control value
   * @param currentFilePath File path of the launch profile these http or https launch profile urls came from
   * @returns Obj if yes or null if not
   */
  private IsUrlTakenByAnother(
    controlValue: string,
    currentFilePath: string
  ): Observable<{ launchUrl: string } | null> {
    const ewindow = window as unknown as EWindow;

    if (!ewindow.electronAPI.works()) {
      return of({ launchUrl: 'Electron API failed' });
    }

    return from(
      ewindow.electronAPI.readFiles(this.store.getSelectedDirectory()!, {
        fileNames: new Set(['launchsettings.json']),
        fileExts: new Set(),
        ignoreDirs: new Set(),
        ignoreFileNames: new Set(),
      })
    ).pipe(
      map((allJsonFiles) =>
        allJsonFiles.filter(
          (file) =>
            file.filePath.toLowerCase() !== currentFilePath.toLowerCase()
        )
      ),
      switchMap((launchSettingsFiles) =>
        from(launchSettingsFiles).pipe(
          mergeMap((file) =>
            from(ewindow.electronAPI.readFile(file.filePath)).pipe(
              map((fileContent) => ({
                conflict: this.checkLaunchSettingsUrls(
                  fileContent,
                  controlValue
                ),
              }))
            )
          ),
          toArray(),
          map((results) => {
            const conflictFound = results.find((r) => r.conflict);
            if (conflictFound) {
              return {
                launchUrl:
                  'HTTP or HTTPS URL already used by another launch profile',
              };
            }
            return null;
          })
        )
      )
    );
  }

  private checkLaunchSettingsUrls(fileContent: string, url: string): boolean {
    try {
      // Normalize input URL for comparison
      const inputUrl = new URL(url);
      const inputHostPort = `${inputUrl.hostname}:${
        inputUrl.port || (inputUrl.protocol === 'https:' ? '443' : '80')
      }`;

      // Match all "applicationUrl": "..." entries (handles multiple profiles)
      const regex = /"applicationUrl"\s*:\s*"([^"]*)"/g;
      let match: RegExpExecArray | null;

      while ((match = regex.exec(fileContent)) !== null) {
        const urls = match[1].split(';').map((u) => u.trim());

        for (const u of urls) {
          try {
            const parsed = new URL(u);
            const hostPort = `${parsed.hostname}:${
              parsed.port || (parsed.protocol === 'https:' ? '443' : '80')
            }`;

            // Check if host:port matches regardless of http/https
            if (hostPort.toLowerCase() === inputHostPort.toLowerCase()) {
              return true;
            }
          } catch {
            continue; // Skip invalid URLs
          }
        }
      }

      return false; // No conflicts found
    } catch {
      return false; // If parsing fails, assume no conflict
    }
  }

  /**
   *  Saves new launmch urls to the orgina;l file and othe files that refrence it
   */
  async saveLaunchUrls() {
    if (!this.err && !this.httpControl.valid && !this.httpsControl.valid) {
      return;
    }
    let ewidnow = window as unknown as EWindow;
    if (!ewidnow.electronAPI.works()) {
      return;
    }

    this.isSaving = true;

    var newHttpUrl = this.httpControl.value;
    var newHttpsUrl = this.httpsControl.value;

    var fileContent = await ewidnow.electronAPI.readFile(this.item.filePath);
    var res = extractUrlsFromLaunchSettings(fileContent);
    if (!res.success) {
      this.isSaving = false;
      return;
    }

    var previousHttpUrl = res.httpUrl;
    var previousHttpsUrl = res.httpsUrl;

    var updatedFileContent = this.switchUrls(
      fileContent,
      previousHttpUrl!,
      previousHttpsUrl!,
      newHttpUrl!,
      newHttpsUrl!
    );
    await ewidnow.electronAPI.overWriteFile(
      this.item.filePath,
      updatedFileContent
    );

    // change all other

    var alljsonFiles = await ewidnow.electronAPI.readFiles(
      this.store.getSelectedDirectory()!,
      {
        fileNames: new Set([
          'appsettings.json',
          'appsettings.development.json',
        ]),
        fileExts: new Set(),
        ignoreDirs: new Set(),
        ignoreFileNames: new Set(),
      }
    );

    var filtered = alljsonFiles.filter((file) => {
      const filePath = file.filePath.toLowerCase();
      return filePath !== this.item.filePath.toLowerCase();
    });

    for (var f of filtered) {
      var fc = await ewidnow.electronAPI.readFile(f.filePath);
      var updated = this.replaceUrlsExact(
        fc,
        previousHttpUrl,
        previousHttpsUrl,
        newHttpUrl!,
        newHttpsUrl!
      );
      await ewidnow.electronAPI.overWriteFile(f.filePath, updated);
    }

    this.isSaving = false;
  }

  switchUrls(
    fileContent: string,
    oldHttp: string,
    oldHttps: string,
    newHttp: string,
    newHttps: string
  ): string {
    // Escape regex special characters from the old URLs
    const escapeRegex = (str: string) =>
      str.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');

    const oldHttpPattern = new RegExp(escapeRegex(oldHttp), 'g');
    const oldHttpsPattern = new RegExp(escapeRegex(oldHttps), 'g');

    // Replace HTTPS first to avoid partial replacement issues
    let updatedContent = fileContent.replace(oldHttpsPattern, newHttps);
    updatedContent = updatedContent.replace(oldHttpPattern, newHttp);

    return updatedContent;
  }

  /**
   * Alternative version that replaces URLs using exact string matching.
   * Faster and simpler than regex version, but only does exact matches.
   *
   * @param content - The string content to search and replace URLs in
   * @param oldHttpUrl - The old HTTP URL to be replaced (optional)
   * @param oldHttpsUrl - The old HTTPS URL to be replaced (optional)
   * @param newHttpUrl - The new HTTP URL to replace with (optional)
   * @param newHttpsUrl - The new HTTPS URL to replace with (optional)
   * @returns The updated content with URLs replaced
   * @throws {Error} When content is not a string
   *
   * @example
   * ```typescript
   * const content = 'Check out http://example.com/page';
   * const result = replaceUrlsExact(
   *   content,
   *   'http://example.com',
   *   undefined,
   *   'http://newsite.com',
   *   undefined
   * );
   * // Returns: 'Check out http://newsite.com/page'
   * ```
   */
  replaceUrlsExact(
    content: string,
    oldHttpUrl?: string,
    oldHttpsUrl?: string,
    newHttpUrl?: string,
    newHttpsUrl?: string
  ): string {
    if (typeof content !== 'string') {
      throw new Error('Content must be a string');
    }

    let updatedContent: string = content;

    // Replace HTTP URL - exact matches only
    if (oldHttpUrl && newHttpUrl) {
      updatedContent = updatedContent.split(oldHttpUrl).join(newHttpUrl);
    }

    // Replace HTTPS URL - exact matches only
    if (oldHttpsUrl && newHttpsUrl) {
      updatedContent = updatedContent.split(oldHttpsUrl).join(newHttpsUrl);
    }

    return updatedContent;
  }
}
