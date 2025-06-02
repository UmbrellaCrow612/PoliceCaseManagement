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
   * Generic method to handle permission toggle changes
   * @param key The name of the permission field
   * @param event The toggle event
   */
  onPermissionToggleChange(
    key: keyof CasePermission,
    event: MatSlideToggleChange
  ) {
    this.disbaleItem = true;
    const updatedPermission = {
      ...this.permissionData(),
      [key]: event.checked,
    };

    this.caseService.updatePermission(updatedPermission).subscribe({
      next: () => {
        this.disbaleItem = false;
        this.snackBar.open(
          `Changed ${this.permissionData().userName}'s "${key}" permission`,
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
}
