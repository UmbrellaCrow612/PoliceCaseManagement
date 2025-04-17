import {
  Component,
  inject,
  InjectionToken,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { MatButtonModule } from '@angular/material/button';

import {
  MAT_DIALOG_DATA,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import { timer } from 'rxjs';
import { SystemIncidentTypeDialogService } from '../../services/system-incident-type-dialog.service';
import { IncidentTypeService } from '../../../../../core/incident-type/services/incident-type-service.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-system-incident-type-delete-dialog',
  imports: [MatDialogModule, MatButtonModule],
  templateUrl: './system-incident-type-delete-dialog.component.html',
  styleUrl: './system-incident-type-delete-dialog.component.css',
})
export class SystemIncidentTypeDeleteDialogComponent implements OnInit {
  constructor(
    private systemIncidentTypeDialogService: SystemIncidentTypeDialogService,
    private incidentTypeService: IncidentTypeService,
    private snackBar: MatSnackBar
  ) {}

  data: { incidentTypeId: string } = inject(MAT_DIALOG_DATA);
  private readonly dialogRef = inject(
    MatDialogRef<SystemIncidentTypeDeleteDialogComponent>
  );

  ngOnInit(): void {
    timer(3500).subscribe(() => {
      this.isDeleteButtonDisabled = false;
    });
  }

  isDeleteButtonDisabled = true;
  errorMessage: string | null = null;

  deletedClick() {
    if (!this.data.incidentTypeId) {
      return;
    }

    this.isDeleteButtonDisabled = true;
    this.errorMessage = null;

    this.incidentTypeService.delete(this.data.incidentTypeId).subscribe({
      next: () => {
        this.systemIncidentTypeDialogService.DELETEDSubject.next(true);
        this.dialogRef.close();
        this.isDeleteButtonDisabled = false;

        this.snackBar.open('Deleted incident type from system', 'Close', {
          duration: 10000,
        });
      },
      error: () => {
        this.isDeleteButtonDisabled = false;
        this.errorMessage = 'Failed';
      },
    });
  }
}
