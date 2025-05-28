import { Component, OnInit } from '@angular/core';
import { CaseService } from '../../../../core/cases/services/case.service';
import { ActivatedRoute } from '@angular/router';
import { CaseAttachment } from '../../../../core/cases/type';
import { formatBackendError } from '../../../../core/app/errors/formatError';
import { CommonModule } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { AddCaseFileAttachmentDialogComponent } from './components/add-case-file-attachment-dialog/add-case-file-attachment-dialog.component';
import { MatButtonModule } from '@angular/material/button';
import { BackNavigationButtonComponent } from '../../../../core/components/back-navigation-button/back-navigation-button.component';

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
    private readonly dialog: MatDialog
  ) {}
  ngOnInit(): void {
    this.caseId = this.active.snapshot.paramMap.get('caseId');
    if (!this.caseId) {
      this.error = 'Failed to get case ID from URL';
      return;
    }

    this.fetchData();
  }

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
}
