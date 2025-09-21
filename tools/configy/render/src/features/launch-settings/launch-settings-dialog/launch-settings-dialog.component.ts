import { Component, inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { EWindow, ReadFileInfo } from '../../../types';
import { MatButtonModule } from '@angular/material/button';
import { extractUrlsFromLaunchSettings, isValidUrl } from '../../../util';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { StoreService } from '../../../store.service';
import { from, map, mergeMap, Observable, of, switchMap, toArray } from 'rxjs';

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
      ewindow.electronAPI.readFiles(this.store.getSelectedDirectory()!, [
        '.json',
      ])
    ).pipe(
      map((allJsonFiles) =>
        allJsonFiles.filter(
          (file) =>
            file.fileName.toLowerCase() === 'launchsettings.json' &&
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
}
