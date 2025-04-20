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
import { RestrictedUser, User } from '../../../../core/user/type';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { UserService } from '../../../../core/user/services/user.service';
import { debounceTime, Subject } from 'rxjs';
import { CommonModule } from '@angular/common';

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
  constructor(private userService: UserService) {}

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

  // TODO - when sending - find the id from filtered users using the username and send that not the username
}
