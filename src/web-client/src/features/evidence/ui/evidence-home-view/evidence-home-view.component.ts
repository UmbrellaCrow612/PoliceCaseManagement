import { Component, inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { UploadEvidenceDialogComponent } from './components/upload-evidence-dialog/upload-evidence-dialog.component';

@Component({
  selector: 'app-evidence-home-view',
  imports: [],
  templateUrl: './evidence-home-view.component.html',
  styleUrl: './evidence-home-view.component.css',
})
export class EvidenceHomeViewComponent {
  private readonly dialog = inject(MatDialog);

  /**
   * Runs when the upload button is clicked
   */
  uploadClicked() {
    this.dialog.open(UploadEvidenceDialogComponent, {
      width: '100%',
      maxWidth: '600px',
      height: '100%',
      maxHeight: '600px',
    });
  }
}
