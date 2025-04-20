import { Component, OnDestroy, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  PristineChangeEvent,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import {
  Validator_containsOnlyNumeric,
  Validator_password,
} from '../../../../../core/app/validators/controls';
import { MatButtonModule } from '@angular/material/button';
import { debounceTime, filter, Observable, Subscription, timer } from 'rxjs';
import { CanComponentDeactivate } from '../../../../../core/app/guards/canDeactivateGuard';
import { UserService } from '../../../../../core/user/services/user.service';
import { isEmail } from '../../../../../core/app/validators/isEmail';
import { isNumeric } from '../../../../../core/app/validators/isNumeric';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-user-management-create-view',
  imports: [
    ReactiveFormsModule,
    MatInputModule,
    MatFormFieldModule,
    MatIconModule,
    MatButtonModule,
  ],
  templateUrl: './user-management-create-view.component.html',
  styleUrl: './user-management-create-view.component.css',
})
export class UserManagementCreateViewComponent
  implements OnInit, OnDestroy, CanComponentDeactivate
{
  constructor(
    private userService: UserService,
    private snackBar: MatSnackBar,
    private active: ActivatedRoute,
    private router: Router
  ) {}
  hasUnsavedChanges = false;
  showPassword = false;
  passwordInputType = 'password';
  userFormValueChangeSubscription: Subscription | null = null;
  userFormUserNameInputChangeSubscription: Subscription | null = null;
  userFormEmailInputChangeSubscription: Subscription | null = null;
  userFormPhoneNumberInputChangeSubscription: Subscription | null = null;
  isSendingCreatingUserRequest = false;

  createUserForm = new FormGroup({
    userName: new FormControl<string | null>(null, [
      Validators.required,
      Validators.maxLength(30),
    ]),
    email: new FormControl<string | null>(null, [
      Validators.required,
      Validators.email,
    ]),
    phoneNumber: new FormControl<string | null>(null, [
      Validators.required,
      Validator_containsOnlyNumeric,
    ]),
    password: new FormControl<string | null>(null, [
      Validators.required,
      Validator_password,
    ]),
  });

  ngOnInit(): void {
    this.userFormValueChangeSubscription = this.createUserForm.events.subscribe(
      (event) => {
        if (event instanceof PristineChangeEvent) {
          if (!event.pristine) {
            this.hasUnsavedChanges = true;
          } else {
            this.hasUnsavedChanges = false;
          }
        }
      }
    );

    this.userFormUserNameInputChangeSubscription =
      this.createUserForm.controls.userName.valueChanges
        .pipe(
          debounceTime(500),
          filter((val) => val?.length! > 1)
        )
        .subscribe((val) => {
          this.userService.isUsernameTaken({ username: val! }).subscribe({
            next: () => {
              this.createUserForm.controls.userName.setErrors(null);
            },
            error: () => {
              this.createUserForm.controls.userName.setErrors({
                usernameTaken: true,
              });
            },
          });
        });

    this.userFormEmailInputChangeSubscription =
      this.createUserForm.controls.email.valueChanges
        .pipe(
          debounceTime(500),
          filter((val) => val?.length! > 1 && isEmail(val ?? ''))
        )
        .subscribe((val) => {
          this.userService.isEmailTaken({ email: val! }).subscribe({
            next: () => {
              this.createUserForm.controls.email.setErrors(null);
            },
            error: () => {
              this.createUserForm.controls.email.setErrors({
                emailTaken: true,
              });
            },
          });
        });

    this.userFormPhoneNumberInputChangeSubscription =
      this.createUserForm.controls.phoneNumber.valueChanges
        .pipe(
          debounceTime(500),
          filter((val) => val?.length! > 1 && isNumeric(val ?? ''))
        )
        .subscribe((val) => {
          this.userService.isPhoneNumberTaken({ phoneNumber: val! }).subscribe({
            next: () => {
              this.createUserForm.controls.phoneNumber.setErrors(null);
            },
            error: () => {
              this.createUserForm.controls.phoneNumber.setErrors({
                phoneNumberTaken: true,
              });
            },
          });
        });
  }

  togglePasswordInputVis() {
    this.showPassword = !this.showPassword;
    if (this.showPassword) {
      this.passwordInputType = 'text';
    } else {
      this.passwordInputType = 'password';
    }
  }

  cancelClick() {
    this.createUserForm.reset(
      {
        email: '',
        password: '',
        phoneNumber: '',
        userName: '',
      },
      {
        emitEvent: true,
      }
    );
  }

  createClick() {
    if (this.createUserForm.valid) {
      this.isSendingCreatingUserRequest = true;

      this.userService
        .createUser({
          email: this.createUserForm.controls.email.getRawValue()!,
          password: this.createUserForm.controls.password.getRawValue()!,
          phoneNumber: this.createUserForm.controls.phoneNumber.getRawValue()!,
          userName: this.createUserForm.controls.userName.getRawValue()!,
        })
        .subscribe({
          next: (response) => {
            this.snackBar.open(
              'Created user successfully. Navigating...',
              'Close',
              {
                duration: 3000,
              }
            );
            this.isSendingCreatingUserRequest = false;
            this.cancelClick();
            timer(3000).subscribe(() => {
              this.router.navigate(['../', 'users', response.id], {
                relativeTo: this.active,
              });
            });
          },
          error: (err: HttpErrorResponse) => {
            let code = err.error[0]?.code;
            this.snackBar.open(`Failed to create user code: ${code}`, 'Close', {
              duration: 10000,
            });
            this.isSendingCreatingUserRequest = false;
          },
        });
    }
  }

  canDeactivate(): boolean | Observable<boolean> {
    if (this.hasUnsavedChanges) {
      return confirm('You have unsaved changes. Do you really want to leave?');
    }
    return true;
  }

  ngOnDestroy(): void {
    if (this.userFormValueChangeSubscription) {
      this.userFormValueChangeSubscription.unsubscribe();
    }
    if (this.userFormUserNameInputChangeSubscription) {
      this.userFormUserNameInputChangeSubscription.unsubscribe();
    }
    if (this.userFormEmailInputChangeSubscription) {
      this.userFormEmailInputChangeSubscription.unsubscribe();
    }
    if (this.userFormPhoneNumberInputChangeSubscription) {
      this.userFormPhoneNumberInputChangeSubscription.unsubscribe();
    }
  }
}
