import { Component, inject, OnInit } from '@angular/core';
import { CaseService } from '../../../../core/cases/services/case.service';
import { ActivatedRoute } from '@angular/router';
import { CaseAttachment } from '../../../../core/cases/type';
import { formatBackendError } from '../../../../core/app/errors/formatError';
import { CommonModule } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { AddCaseFileAttachmentDialogComponent } from './components/add-case-file-attachment-dialog/add-case-file-attachment-dialog.component';
import { MatButtonModule } from '@angular/material/button';
import { BackNavigationButtonComponent } from '../../../../core/components/back-navigation-button/back-navigation-button.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { hasRequiredRole } from '../../../../core/authentication/utils/hasRequiredRole';
import { UserRoles } from '../../../../core/authentication/roles';
import { UserService } from '../../../../core/user/services/user.service';

@Component({
  selector: 'app-cases-id-attachments-view',
  imports: [CommonModule, MatButtonModule, BackNavigationButtonComponent],
  templateUrl: './cases-id-attachments-view.component.html',
  styleUrl: './cases-id-attachments-view.component.css',
})
export class CasesIdAttachmentsViewComponent implements OnInit {
  constructor(
    private readonly caseService: CaseService,
    private readonly active: ActivatedRoute,
    private readonly dialog: MatDialog,
    private readonly snackBar: MatSnackBar
  ) {}
  ngOnInit(): void {
    this.caseId = this.active.snapshot.paramMap.get('caseId');
    if (!this.caseId) {
      this.error = 'Failed to get case ID from URL';
      return;
    }

    this.fetchData();
  }

  hasRoles = hasRequiredRole;
  systemRoles = UserRoles;
  currentUserRoles = inject(UserService).ROLES!;

  attachments: CaseAttachment[] = [];

  caseId: string | null = null;

  isLoading = false;
  error: string | null = null;
  fetchData() {
    if (!this.caseId) {
      this.error = 'Failed to get case ID from URL';
      return;
    }

    this.isLoading = true;
    this.error = null;

    this.caseService.getAttachments(this.caseId).subscribe({
      next: (files) => {
        this.attachments = files;
        this.isLoading = false;
      },
      error: (err) => {
        this.error = formatBackendError(err);
        this.isLoading = false;
      },
    });
  }

  addCaseFileAttachmentClicked() {
    this.dialog
      .open(AddCaseFileAttachmentDialogComponent, {
        width: '100%',
        maxWidth: '500px',

        data: {
          caseId: this.caseId,
        },
      })
      .afterClosed()
      .subscribe(() => {
        this.fetchData();
      });
  }

  downloadError = '';
  isDownloadingAttachment = false;
  downloadAttachamentClicked(item: CaseAttachment) {
    this.isDownloadingAttachment = true;

    this.caseService.downloadAttachment(item.id).subscribe({
      next: (response) => {
        const blob = response.body as Blob;

        const contentDisposition = response.headers.get('Content-Disposition');
        let filename = 'downloaded-file';

        if (contentDisposition) {
          const filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
          const matches = filenameRegex.exec(contentDisposition);
          if (matches != null && matches[1]) {
            filename = matches[1].replace(/['"]/g, ''); // Remove quotes
            // Handle URI-encoded filename e.g. filename*=UTF-8''my%20file.pdf
            if (filename.startsWith("UTF-8''")) {
              filename = decodeURIComponent(filename.substring(7));
            }
          }
        }

        // Create a download link and simulate a click
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = filename;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);

        this.isDownloadingAttachment = false;
      },
      error: (err) => {
        this.downloadError = formatBackendError(err);
        this.isDownloadingAttachment = false;

        this.snackBar.open(this.downloadError, 'Close');
      },
    });
  }

  isDeletingAttachment = false;
  deleteAttachmentClicked(item: CaseAttachment) {
    this.isDeletingAttachment = true;
    this.caseService.deleteAttachment(item.id).subscribe({
      next: () => {
        this.isDeletingAttachment = false;
        this.fetchData();
      },
      error: (err) => {
        this.snackBar.open(formatBackendError(err), 'Close');
      },
    });
  }
}
