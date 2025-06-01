import { Component, input } from '@angular/core';
import {
  MatSlideToggleChange,
  MatSlideToggleModule,
} from '@angular/material/slide-toggle';
import { CasePermission } from '../../../../../../core/cases/type';
import { CaseService } from '../../../../../../core/cases/services/case.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { formatBackendError } from '../../../../../../core/app/errors/formatError';

@Component({
  selector: 'app-case-permission-item',
  imports: [MatSlideToggleModule],
  templateUrl: './case-permission-item.component.html',
  styleUrl: './case-permission-item.component.css',
})
export class CasePermissionItemComponent {
  constructor(
    private readonly caseService: CaseService,
    private readonly snackBar: MatSnackBar
  ) {}
  permissionData = input.required<CasePermission>();

  /**
   * used to disbale item while one value changes in the mat slide
   */
  disbaleItem = false;

  /**
   * Fires off whena specific permission is changed i.e when the slide toggle changes for can edit
   * @param event The angular material slide toggle event contaning the info
   * @param permission The permission it was fired for
   */
  canEditPermissionSlideToggleValueChangedEventListner(
    event: MatSlideToggleChange
  ) {
    this.disbaleItem = true;
    let copy = this.permissionData();

    copy.canEdit = event.checked;

    this.caseService.updatePermission(copy).subscribe({
      next: () => {
        this.disbaleItem = false;
        this.snackBar.open(
          `Changed ${this.permissionData().userName} edit permissions`,
          'Close',
          { duration: 5000 }
        );
      },
      error: (err) => {
        this.disbaleItem = false;
        this.snackBar.open(formatBackendError(err), 'Close');
      },
    });
  }

  /**
   * Fires off whena specific permission is changed i.e when the slide toggle changes for can assign
   * @param event The angular material slide toggle event contaning the info
   * @param permission The permission it was fired for
   */
  canAssignPermissionSlideToggleValueChangedEventListner(
    event: MatSlideToggleChange
  ) {
    this.disbaleItem = true;
    let copy = this.permissionData();

    copy.canAssign = event.checked;

    this.caseService.updatePermission(copy).subscribe({
      next: () => {
        this.disbaleItem = false;
        this.snackBar.open(
          `Changed ${this.permissionData().userName} assign permissions`,
          'Close',
          { duration: 5000 }
        );
      },
      error: (err) => {
        this.disbaleItem = false;
        this.snackBar.open(formatBackendError(err), 'Close');
      },
    });
  }

  // fix comments

  // whe sending disbale the whole item to stop spaming and use toasts
  // also out event when changed so parent can refresh data
}
