import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { RestrictedUser } from '../../../../core/user/type';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { debounceTime, Subject, timer } from 'rxjs';
import { CommonModule } from '@angular/common';
import { CaseService } from '../../../../core/cases/services/case.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ErrorService } from '../../../../core/app/errors/services/error.service';
import { HttpErrorResponse } from '@angular/common/http';
import { getBusinessErrorCode } from '../../../../core/server-responses/getBusinessErrorCode';
import CODES from '../../../../core/server-responses/codes';
import { ActivatedRoute, Router } from '@angular/router';
import { CreateCase } from '../../../../core/cases/type';
import { SearchUsersSelectComponent } from '../../../../core/user/components/search-users-select/search-users-select.component';

@Component({
  selector: 'app-cases-create-view',
  imports: [
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    MatButtonModule,
    FormsModule,
    MatDatepickerModule,
    MatAutocompleteModule,
    CommonModule,
    SearchUsersSelectComponent,
  ],
  templateUrl: './cases-create-view.component.html',
  styleUrl: './cases-create-view.component.css',
})
export class CasesCreateViewComponent implements OnInit {
  constructor(
    private caseService: CaseService,
    private snackBar: MatSnackBar,
    private errorService: ErrorService,
    private active: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.createCaseForm.controls.caseNumber.valueChanges
      .pipe(debounceTime(1200))
      .subscribe({
        next: () => {
          let value = this.createCaseForm.controls.caseNumber.value;
          if (
            value?.trim() !== '' &&
            !this.createCaseForm.controls.caseNumber.hasError('required') &&
            !this.createCaseForm.controls.caseNumber.hasError('minlength')
          ) {
            this.caseService.isCaseNumberTaken(value!).subscribe({
              error: () => {
                this.createCaseForm.controls.caseNumber.setErrors({
                  caseNumberTaken: true,
                });
              },
            });
          }
        },
      });
  }

  selectReportingOfficerFormFieldStyles = {
    width: '100%',
  };

  createCaseForm = new FormGroup({
    caseNumber: new FormControl<string | null>('', [
      Validators.required,
      Validators.minLength(5),
    ]),
    summary: new FormControl<string | null>(''),
    description: new FormControl<string | null>(null),
    incidentDateTime: new FormControl<Date>(new Date(), [Validators.required]),
    reportingOfficer: new FormControl<RestrictedUser | null>(null, [
      Validators.required,
    ]),
  });

  /**
   * Flag to indicate a case is being created
   */
  creatingCase = false;

  onSubmit() {
    if (this.createCaseForm.valid) {
      this.creatingCase = true;

      let caseToCreate: CreateCase = {
        caseNumber: this.createCaseForm.controls.caseNumber.value!.trim(),
        description: this.createCaseForm.controls.description.value,
        incidentDateTime: this.createCaseForm.controls.incidentDateTime.value!,
        reportingOfficerId:
          this.createCaseForm.controls.reportingOfficer.value?.id!,
        summary: this.createCaseForm.controls.summary.value,
      };

      this.caseService.create(caseToCreate).subscribe({
        next: (resposne) => {
          this.snackBar.open('Created case', 'Close', { duration: 10000 });

          timer(1200).subscribe(() => {
            this.router.navigate(['../', resposne.id], {
              relativeTo: this.active,
            });
          });
        },
        error: (error: HttpErrorResponse) => {
          this.creatingCase = false;

          let code = getBusinessErrorCode(error);

          switch (code) {
            case CODES.CASE_NUMBER_TAKEN:
              this.createCaseForm.controls.caseNumber.setErrors({
                caseNumberTaken: true,
              });
              break;

            default:
              this.errorService.HandleDisplay(error);
              break;
          }
        },
      });
    }
  }
}
