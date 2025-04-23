import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import {
  MAT_SNACK_BAR_DATA,
  MatSnackBarModule,
  MatSnackBarRef,
} from '@angular/material/snack-bar';
import { formatBackendError } from '../../formatError';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-error-snack-bar',
  imports: [MatSnackBarModule, CommonModule, MatButtonModule],
  templateUrl: './error-snack-bar.component.html',
  styleUrl: './error-snack-bar.component.css',
})
export class ErrorSnackBarComponent {
  snackBarRef = inject(MatSnackBarRef);
  data = inject(MAT_SNACK_BAR_DATA);

  mess = formatBackendError(this.data);
}
