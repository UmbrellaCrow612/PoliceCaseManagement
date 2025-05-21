import { Component, input } from '@angular/core';
import { CaseAction } from '../../../../../../core/cases/type';
import { CommonModule } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { CaseActionDialogDetailsComponent } from '../case-action-dialog-details/case-action-dialog-details.component';

@Component({
  selector: 'app-case-action-details',
  imports: [CommonModule],
  templateUrl: './case-action-details.component.html',
  styleUrl: './case-action-details.component.css',
})
export class CaseActionDetailsComponent {
  constructor(private dialog: MatDialog) {}
  details = input.required<CaseAction>();

  onClicked() {
    this.dialog.open(CaseActionDialogDetailsComponent, {
      data: { id: this.details().id },
      width: '100%',
      maxWidth: '500px',
    });
  }
}
