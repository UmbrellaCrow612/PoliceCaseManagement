import { Component } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

@Component({
  selector: 'app-login-view',
  imports: [ReactiveFormsModule],
  templateUrl: './login-view.component.html',
  styleUrl: './login-view.component.css',
})
export class LoginViewComponent {
  loginForm = new FormGroup({
    email: new FormControl(null, [Validators.email]),
    username: new FormControl(null),
    password: new FormControl(''),
  });

  onSubmit() {}
}
