import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { RestrictedUser } from '../../../../core/user/type';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { UserService } from '../../../../core/user/services/user.service';
import { debounceTime, Subject } from 'rxjs';
import { CommonModule } from '@angular/common';
import { CaseService } from '../../../../core/cases/services/case.service';
import { MatSnackBar } from '@angular/material/snack-bar';

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
  providers: [provideNativeDateAdapter()],
})
export class CasesCreateViewComponent implements OnInit {
  constructor(
    private userService: UserService,
    private caseService: CaseService,
    private snackBar: MatSnackBar
  ) {}

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
            // some rr state
          },
        });
      } else {
        this.fetchingUsers = false;
        this.filteredUsers = [];
      }
    });
  }

  /**
   * Display any error
   */
  errorMessage: string | null = null;

  createCaseForm = new FormGroup({
    caseNumber: new FormControl<string | null>(null),
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
      this.errorMessage = null;

      let _caseNumber = this.createCaseForm.controls.caseNumber.value;
      if (_caseNumber?.trim() == '') {
        _caseNumber = null; // stop sending empty case number backend
      }

      this.caseService
        .create({
          caseNumber: _caseNumber,
          description: this.createCaseForm.controls.description.value,
          incidentDateTime:
            this.createCaseForm.controls.incidentDateTime.value!,
          reportingOfficerId: reportingOfficerId,
          summary: this.createCaseForm.controls.summary.value,
        })
        .subscribe({
          next: () => {
            this.creatingCase = false;
            this.snackBar.open('Created case', 'Close', { duration: 10000 });
          },
          error: () => {
            this.creatingCase = false;
            this.errorMessage = 'Failed';
          },
        });
    }
  }
}
