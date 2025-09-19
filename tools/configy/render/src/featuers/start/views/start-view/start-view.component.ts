import { Component, ElementRef, viewChild } from '@angular/core';
import { DotNetFile, EWindow, ExtractedUrls } from '../../../../types';
import { extractLaunchUrls, validUrl } from '../../../../util';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-start-view',
  imports: [ReactiveFormsModule],
  templateUrl: './start-view.component.html',
  styleUrl: './start-view.component.css',
})
export class StartViewComponent {
  /**
   * Folder picked
   */
  solutionFolder: string | null = null;

  /**
   * Holds any loading state
   */
  loading = false;

  /**
   * Holds any error state
   */
  error: string | null = null;

  /**
   * List of files read from solution folder
   */
  files: DotNetFile[] = [];

  /**
   * Current item to show in the dialog
   */
  dialogItem: DotNetFile | null = null;

  /**
   * Current dialog items launch urls extracted
   */
  dialogExctractedUrl: ExtractedUrls | null = null;

  /**
   * Ref to dialog html child
   */
  dialog = viewChild<ElementRef<HTMLDialogElement>>('dialog');

  httpControl = new FormControl('', {
    validators: [Validators.required, validUrl],
  });
  httpsControl = new FormControl('', {
    validators: [Validators.required, validUrl],
  });

  /**
   * Handles loading and opening desktop solution folder and files
   */
  async loadSolutionFolder() {
    this.loading = true;
    this.error = null;
    this.solutionFolder = null;
    this.files = [];

    /**
     * Regular window obj but we will acess electron api
     */
    let _window = window as unknown as EWindow;

    if (_window.electronAPI.works()) {
      var res = await _window.electronAPI.openDirectory();

      if (res.canceled) {
        this.error = 'Cancel open folder try again';
        this.loading = false;
        return;
      } else {
        this.solutionFolder = res.filePaths[0];
        this.loading = false;
      }
    } else {
      this.error = 'Failed to open directory somthing is wrong';
    }
  }

  /**
   * Cancel a selected solution folder and resets sate
   */
  cancel() {
    this.error = null;
    this.solutionFolder = null;
    this.files = [];
  }

  /**
   * Handles reading all files for configy and processing them
   */
  async readFiles() {
    this.loading = true;
    this.error = null;
    this.files = [];

    /**
     * Regular window obj but we will acess electron api
     */
    let _window = window as unknown as EWindow;

    if (_window.electronAPI.works() && this.solutionFolder) {
      this.files = await _window.electronAPI.readDotNetFiles(
        this.solutionFolder
      );
      this.loading = false;
    } else {
      this.error = 'Desktop API not working or solution folder is empty';
    }
  }

  /**
   * Handles closing dialog
   */
  CloseDialog() {
    this.dialog()?.nativeElement.close();
    this.dialogItem = null;
  }

  /**
   * Shows a file in dialog
   * @param item The file to show in the dialog
   */
  showItem(item: DotNetFile) {
    this.dialogItem = item;
    var res = extractLaunchUrls(item.fileContent);
    if (res.success) {
      this.dialogExctractedUrl = res.data;
      this.httpControl.setValue(res.data.http);
      this.httpsControl.setValue(res.data.https);
    } else {
      this.dialogExctractedUrl = null;
    }
    this.dialog()?.nativeElement.showModal();
  }
}
