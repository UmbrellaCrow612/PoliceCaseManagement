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
import { UserService } from '../../../../core/user/services/user.service';
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
  ],
  templateUrl: './cases-create-view.component.html',
  styleUrl: './cases-create-view.component.css',
})
export class CasesCreateViewComponent implements OnInit {
  constructor(
    private userService: UserService,
    private caseService: CaseService,
    private snackBar: MatSnackBar,
    private errorService: ErrorService,
    private active: ActivatedRoute,
    private router: Router
  ) { }

  @ViewChild('input') input: ElementRef<HTMLInputElement> | null = null;

  ngOnInit(): void {
    this.filterSubject.pipe(debounceTime(1000)).subscribe(() => {
      let searchTerm = this.input?.nativeElement.value;
      if (searchTerm && searchTerm.length > 0) {
        this.userService.searchUsersByUsername(searchTerm).subscribe({
          next: (users) => {
            this.filteredUsers = users;
            this.fetchingUsers = false;
          },
          error: () => {
            this.fetchingUsers = false;
            this.filteredUsers = [];
          },
        });
      } else {
        this.fetchingUsers = false;
        this.filteredUsers = [];
      }
    });

    this.createCaseForm.controls.caseNumber.valueChanges.pipe(debounceTime(1200)).subscribe({
      next: () => {
        let value = this.createCaseForm.controls.caseNumber.value
        if (value?.trim() !== "" && !this.createCaseForm.controls.caseNumber.hasError("required") && !this.createCaseForm.controls.caseNumber.hasError("minlength")) {
          this.caseService.isCaseNumberTaken(value!).subscribe({
            error: () => {
              this.createCaseForm.controls.caseNumber.setErrors({
                caseNumberTaken: true
              })
            }
          })
        }
      }
    })
  }



  createCaseForm = new FormGroup({
    caseNumber: new FormControl<string | null>("", [Validators.required, Validators.minLength(5)]),
    summary: new FormControl<string | null>(''),
    description: new FormControl<string | null>(null),
    incidentDateTime: new FormControl<Date>(new Date(), [Validators.required]),
    reportingOfficerUserName: new FormControl('', [Validators.required]),
  });

  /**
   * Users array matching the search params the user typed
   */
  filteredUsers: RestrictedUser[] = [];

  /**
   * flag to indicate that users are being fetched when the user types in the
   * assign officers field.
   */
  fetchingUsers = false;

  /**
   * Used for searching debounce time.
   */
  private filterSubject = new Subject<void>();
  /**
   * Get backend users based on the auto complete query for user names of the officers in the system
   */
  filter() {
    this.fetchingUsers = true;
    this.filterSubject.next();
  }

  /**
   * Flag to indicate a case is being created
   */
  creatingCase = false;

  onSubmit() {
    if (this.createCaseForm.valid) {
      let reportingOfficerId = this.filteredUsers.find(
        (x) =>
          x.userName ===
          this.createCaseForm.controls.reportingOfficerUserName.value
      )?.id;

      if (!reportingOfficerId) {
        return; // do some err check
      }

      this.creatingCase = true;

      let caseToCreate: CreateCase = {
        caseNumber: this.createCaseForm.controls.caseNumber.value!.trim(),
        description: this.createCaseForm.controls.description.value,
        incidentDateTime: this.createCaseForm.controls.incidentDateTime.value!,
        reportingOfficerId: reportingOfficerId,
        summary: this.createCaseForm.controls.summary.value
      }

      this.caseService
        .create(caseToCreate)
        .subscribe({
          next: (resposne) => {
            this.snackBar.open('Created case', 'Close', { duration: 10000 });

            timer(1200).subscribe(() => {
              this.router.navigate(["../", resposne.id], { relativeTo: this.active })
            })
          },
          error: (error: HttpErrorResponse) => {
            this.creatingCase = false;

            let code = getBusinessErrorCode(error)

            switch (code) {
              case CODES.CASE_NUMBER_TAKEN:
                this.createCaseForm.controls.caseNumber.setErrors({
                  caseNumberTaken: true
                })
                break;

              default:
                this.errorService.HandleDisplay(error)
                break;
            }
          },
        });
    }
  }
}
