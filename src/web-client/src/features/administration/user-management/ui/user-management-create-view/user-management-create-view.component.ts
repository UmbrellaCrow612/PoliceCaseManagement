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
import { Observable, Subscription } from 'rxjs';
import { CanComponentDeactivate } from '../../../../../core/app/guards/canDeactivateGuard';

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
  hasUnsavedChanges = false;
  showPassword = false;
  passwordInputType = 'password';
  userFormValueChangeSubscription: Subscription | null = null;

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

  /**
   * TODO link to backend and endpoints for checking if certain stuff are taken etc
   */

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
  }
}
