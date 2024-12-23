import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthenticationService } from '../core/authentication/services/authentication.service';
import { LoginCredentials } from '../core/authentication/types';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  res:any = null;
  cred : LoginCredentials = {userName: 'ererervrevev', email: "userververver@example.com", password: "Password@123"};

  constructor(private authService : AuthenticationService) {
    this.authService.Login(this.cred).subscribe((res) => {
      this.res = res;
    });
  }
}
