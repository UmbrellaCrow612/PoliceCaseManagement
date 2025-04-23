import { MatSnackBar } from '@angular/material/snack-bar';
import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { HttpErrorResponse } from '@angular/common/http';
import { StatusCodes } from '../../../http/codes/status-codes';
import { ErrorDialogComponent } from '../components/error-dialog/error-dialog.component';
import { ErrorSnackBarComponent } from '../components/error-snack-bar/error-snack-bar.component';

@Injectable({
  providedIn: 'root',
})
export class ErrorService {
  constructor(private snackBar: MatSnackBar, private dialog: MatDialog) {}

  /**
   * Handle and display errors from the backend in a standard format.
   * @param error HTTP error response
   */
  HandleDisplay(error: HttpErrorResponse) {
    let status = error.status;
    if (status === StatusCodes.INTERNAL_SERVER_ERROR) {
      this.dialog.open(ErrorDialogComponent, { data: error });
    } else {
      this.snackBar.openFromComponent(ErrorSnackBarComponent, { data: error });
    }
  }
}
