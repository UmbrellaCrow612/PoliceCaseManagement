import {
  Component,
  ElementRef,
  forwardRef,
  inject,
  input,
  OnInit,
  ViewChild,
} from '@angular/core';
import { RestrictedUser } from '../../type';
import {
  ControlValueAccessor,
  FormControl,
  NG_VALUE_ACCESSOR,
  ReactiveFormsModule,
} from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import {
  MatAutocompleteModule,
  MatAutocompleteSelectedEvent,
} from '@angular/material/autocomplete';
import { debounceTime, filter, Subject, tap } from 'rxjs';
import { UserService } from '../../services/user.service';
import { CommonModule } from '@angular/common';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-search-users-multi-select',
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatAutocompleteModule,
    MatInputModule,
    CommonModule,
    MatChipsModule,
    MatIconModule,
  ],
  templateUrl: './search-users-multi-select.component.html',
  styleUrl: './search-users-multi-select.component.css',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SearchUsersMultiSelectComponent),
      multi: true,
    },
  ],
})
/**
 * Used as a form control in a reactive form to search and select multiple usersf
 */
export class SearchUsersMultiSelectComponent
  implements ControlValueAccessor, OnInit
{
  private readonly userService = inject(UserService);

  ngOnInit(): void {
    this.eventBus$
      .pipe(
        filter((value) => value !== ''),
        debounceTime(this.DEBOUNCE_TIME_MS)
      )
      .subscribe((searchValue) => {
        this.userService.searchUsersByUsername(searchValue).subscribe({
          next: (users) => {
            let filteredUsers: RestrictedUser[] = [];

            users.forEach((x) => {
              if (!this.SelectedUsers.has(x.id)) {
                filteredUsers.push(x);
              }
            });
            this.SearchedUsers = filteredUsers;

            this.isLoading = false;
          },
          error: (error) => {
            this.error = error;
            this.isLoading = false;
          },
        });
      });
  }
  /**
   * Input field
   */
  @ViewChild('input') input: ElementRef<HTMLInputElement> | null = null;

  /**
   *  Placeholder for the callback function that should be called when the value changes
   */
  private onChange: (value: RestrictedUser[]) => void = () => {};

  /**
   * Placeholder for the callback function that should be called when the control is touched
   */
  private onTouched: () => void = () => {};

  /**
   * This method is called by the Forms API to write a value from the model to the view.
   */
  writeValue(value: RestrictedUser[] | null): void {
    this.SelectedUsers = new Map<string, RestrictedUser>();
    if (value) {
      value?.forEach((x) => {
        this.SelectedUsers.set(x.id, x);
      });
    }
  }

  /**
   * Registers a callback function that should be called when the control's value
   * changes in the UI.
   */
  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  /**
   * Registers a callback function that should be called when the control is "touched".
   */
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  /**
   * This function is called by the forms API when the control's status changes
   * to or from "DISABLED".
   */
  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  /**
   * Runs when a option is selected
   * @param event Event containing the data
   */
  optionSelected(event: MatAutocompleteSelectedEvent) {
    let user = this.SearchedUsers.find((x) => x.userName == event.option.value);
    if (user && !this.SelectedUsers.has(user.id)) {
      this.SelectedUsers.set(user.id, user);
      this.SearchInputControl.setValue(null);
      this.SearchedUsers = this.SearchedUsers.filter(
        (x) => x.userName !== user.userName
      );

      this.onChange(Array.from(this.SelectedUsers.values()));
      this.onTouched();
    } else {
      console.error('User selected failed');
    }
  }

  /**
   * Ran when the user types
   */
  search() {
    let value = this.input?.nativeElement.value.toLowerCase();
    if (value) {
      this.isLoading = true;
      this.error = '';
      this.eventBus$.next(value);
    }
  }

  /**
   * Ran when remove a selected user is clicked
   * @param user The user to remove
   */
  removeSelectedUser(user: RestrictedUser) {
    this.SelectedUsers.delete(user.id);
    this.onChange(Array.from(this.SelectedUsers.values()));
    this.onTouched();
  }

  /**
   * Internal state that holds all selected users as a dictionary map with there ID being the KEY and details as the value
   */
  SelectedUsers = new Map<string, RestrictedUser>();

  /**
   * Sets the forms disabled state
   */
  disabled = false;

  /**
   * State to manage any loading of data
   */
  isLoading = false;

  /**
   * Holds any error state when fetching data
   */
  error = '';

  /**
   * The search inputt field
   */
  SearchInputControl = new FormControl({ value: '', disabled: this.disabled });

  /**
   * List of users fetched
   */
  SearchedUsers: RestrictedUser[] = [];

  /**
   * Time waits until the user stops typing
   */
  private readonly DEBOUNCE_TIME_MS = 1000;

  /**
   * Used to emit input typing events to
   */
  private eventBus$ = new Subject<string>();

  /**
   * Label tag text
   */
  displayText = input.required<string>();
}
