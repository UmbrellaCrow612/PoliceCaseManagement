import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthenticationService } from '../core/authentication/services/authentication.service';
import { LoginCredentials } from '../core/authentication/types';
import { AdamService } from '../core/app/services/adam.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  res:any = null;
  cred : LoginCredentials = {userName: 'ererervrevev', email: "userververver@example.com", password: "Password@123"};

  constructor(private authService : AuthenticationService, private adamService: AdamService) {
    this.authService.Login(this.cred).subscribe((res) => {
      this.res = res;
    });
  }
  ngOnInit(): void {
    this.adamService.initialize();
  }
}
