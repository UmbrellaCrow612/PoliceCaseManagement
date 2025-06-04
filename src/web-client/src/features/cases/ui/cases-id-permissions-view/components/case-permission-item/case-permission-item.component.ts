import { Component, input, OnInit, output } from '@angular/core';
import {
  MatSlideToggleChange,
  MatSlideToggleModule,
} from '@angular/material/slide-toggle';
import {
  CasePermission,
  CasePermissionNames,
} from '../../../../../../core/cases/type';
import { CaseService } from '../../../../../../core/cases/services/case.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { formatBackendError } from '../../../../../../core/app/errors/formatError';
import { hasRequiredPermissions } from '../../../../../../core/authentication/utils/hasRequiredPermissions';

@Component({
  selector: 'app-case-permission-item',
  imports: [MatSlideToggleModule],
  templateUrl: './case-permission-item.component.html',
  styleUrl: './case-permission-item.component.css',
})
export class CasePermissionItemComponent implements OnInit {
  constructor(
    private readonly caseService: CaseService,
    private readonly snackBar: MatSnackBar
  ) {}
  ngOnInit(): void {
    if (
      !hasRequiredPermissions(
        [this.casePermissionNames.canEditPermissions],
        this.currentUserCasePermissions()
      )
    ) {
      this.disbaleItem = true;
    }
  }
  permissionData = input.required<CasePermission>();

  currentUserCasePermissions = input.required<string[]>();
  casePermissionNames = CasePermissionNames;

  /**
   * used to disbale item while one value changes in the mat slide
   */
  disbaleItem = false;

  /**
   * Event fired off when the permission is updated
   */
  permissionUpdatedEvent = output<void>();

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
        this.permissionUpdatedEvent.emit();
      },
      error: (err) => {
        this.disbaleItem = false;
        this.snackBar.open(formatBackendError(err), 'Close');
      },
    });
  }
}
