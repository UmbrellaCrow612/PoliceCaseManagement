import {
  Component,
  ElementRef,
  forwardRef,
  inject,
  input,
  OnInit,
  ViewChild,
} from '@angular/core';
import {
  ControlValueAccessor,
  FormControl,
  NG_VALUE_ACCESSOR,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { UserService } from '../../services/user.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import {
  MatAutocompleteModule,
  MatAutocompleteSelectedEvent,
} from '@angular/material/autocomplete';
import { RestrictedUser } from '../../type';
import { debounceTime, Subject } from 'rxjs';

@Component({
  selector: 'app-search-users-select',
  imports: [
    MatFormFieldModule,
    MatInputModule,
    MatAutocompleteModule,
    ReactiveFormsModule,
  ],
  templateUrl: './search-users-select.component.html',
  styleUrl: './search-users-select.component.css',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SearchUsersSelectComponent),
      multi: true,
    },
  ],
})
/**
 * Used as a form control to search and select a user in a reactive form.
 * It propagates the selected RestrictedUser object to the parent form control.
 */
export class SearchUsersSelectComponent
  implements ControlValueAccessor, OnInit
{
  /**
   * Optionally pass this to make the user select required or not
   */
  requireSelection = input<boolean>(false);

  /**
   * Text to display as the label for the input
   */
  displayText = input.required<string>();

  @ViewChild('input') input: ElementRef<HTMLInputElement> | null = null;

  ngOnInit(): void {
    this.searchSubject.pipe(debounceTime(1000)).subscribe(() => {
      this.fetchUsers();
    });
    this.searchUsersInput.valueChanges.subscribe(() => {
      let searchTerm = this.input?.nativeElement.value.toLowerCase();

      if (searchTerm?.trim() === '') {
        this.selectedUser = null;
        this.onChange(null);
      }
    });

    if (this.requireSelection()) {
      this.searchUsersInput.addValidators(Validators.required);
    }
  }

  writeValue(obj: RestrictedUser | null): void {
    this.selectedUser = obj;
    this.searchUsersInput.setValue(null);
  }

  registerOnChange(fn: (value: RestrictedUser | null) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    if (isDisabled) {
      this.searchUsersInput.disable();
    } else {
      this.searchUsersInput.enable();
    }
  }

  onChange: (value: RestrictedUser | null) => void = () => {};
  onTouched: () => void = () => {};

  private readonly userService = inject(UserService);
  private searchSubject = new Subject<void>();

  /**
   * Internal state of selected user
   */
  selectedUser: RestrictedUser | null = null;

  /**
   * Loading state if it is fetching users
   */
  isLoading = false;

  /**
   * Error state
   */
  error: string | null = null;

  /**
   * Input used to search for users to select
   */
  searchUsersInput = new FormControl('');

  /**
   * All the users fetched for the search
   */
  searchedUsers: RestrictedUser[] = [];

  search() {
    this.isLoading = true;
    this.error = null;
    this.searchSubject.next();
  }

  fetchUsers() {
    let searchTerm = this.input?.nativeElement.value.toLowerCase();
    if (searchTerm && searchTerm.trim() !== '') {
      this.userService.searchUsersByUsername(searchTerm).subscribe({
        next: (users) => {
          this.searchedUsers = users;
          this.isLoading = false;
        },
        error: (err) => {
          this.error = err;
          this.isLoading = false;
        },
      });
    } else {
      this.isLoading = false;
    }
  }

  optionSelected(event: MatAutocompleteSelectedEvent) {
    const selected = this.searchedUsers.find(
      (x) => x.userName === event.option.value
    )!;
    this.selectedUser = selected;
    this.onChange(selected);
    this.onTouched();
  }
}
