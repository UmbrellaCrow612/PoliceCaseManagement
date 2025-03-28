import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { User } from './../../../../../../../core/user/type';
import { Component, input, model, OnInit, output } from '@angular/core';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-user-management-edit-user-details-view',
  imports: [ReactiveFormsModule, MatInputModule, CommonModule],
  templateUrl: './user-management-edit-user-details-view.component.html',
  styleUrl: './user-management-edit-user-details-view.component.css',
})
export class UserManagementEditUserDetailsViewComponent implements OnInit {
  ngOnInit(): void {
    this.userEditForm.disable();

    this.userEditForm.patchValue(
      {
        userName: this.userData()?.userName,
        email: this.userData()?.email,
      },
      {
        emitEvent: false,
        onlySelf: true,
      }
    );

    this.userEditForm.enable();

    this.userEditForm.valueChanges.subscribe((newData) => {
      this.userDataChangesMadeEvent.emit(this.userEditForm.valid);

      this.userData.update((current: any) => {
        return { ...current, ...newData }; // replace with form data value
      });
    });
  }

  userData = model<User | null>(null);

  /**
   * Emits if the user data changes and if the form is still valid
   */
  userDataChangesMadeEvent = output<boolean>();

  userEditForm = new FormGroup({
    userName: new FormControl<string>('', [Validators.required]),
    email: new FormControl<string>('', [Validators.required, Validators.email]),
  });
}
