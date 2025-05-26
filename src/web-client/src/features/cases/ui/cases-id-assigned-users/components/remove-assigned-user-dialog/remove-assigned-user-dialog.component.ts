import { Component, inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import {
  MAT_DIALOG_DATA,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import { CaseService } from '../../../../../../core/cases/services/case.service';
import { formatBackendError } from '../../../../../../core/app/errors/formatError';

@Component({
  selector: 'app-remove-assigned-user-dialog',
  imports: [MatDialogModule, MatButtonModule],
  templateUrl: './remove-assigned-user-dialog.component.html',
  styleUrl: './remove-assigned-user-dialog.component.css',
})
export class RemoveAssignedUserDialogComponent implements OnInit {
  constructor(private readonly caseService: CaseService) {}
  ngOnInit(): void {
    if (!this.data.userId || !this.data.caseId) {
      this.error = 'Props passed to dialog are invalid';
      return;
    }
  }
  /**
   * Data passed to the dialog to perform the actions
   */
  readonly data = inject<{ userId: string; caseId: string }>(MAT_DIALOG_DATA);
  readonly dialogRef = inject(MatDialogRef<RemoveAssignedUserDialogComponent>);

  /**
   * Holds error state
   */
  error: string | null = null;
  /**
   * Loading state for remopving a user
   */
  isRemovingUser = false;
  /**
   * Ran when the user confirms whenm they want to remove the given user
   */
  removeUserClicked() {
    this.error = null;
    this.isRemovingUser = true;

    this.caseService.removeUser(this.data.caseId, this.data.userId).subscribe({
      next: () => {
        this.dialogRef.close();
      },
      error: (err) => {
        this.error = formatBackendError(err);
        this.isRemovingUser = false;
      },
    });
  }
}
