import { Component, OnDestroy, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthenticationService } from '../core/authentication/services/authentication.service';
import { LoginCredentials } from '../core/authentication/types';
import { AdamService } from '../core/app/services/adam.service';
import { JwtService } from '../core/authentication/services/jwt.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit, OnDestroy {
  constructor(private ss : JwtService, private adamService: AdamService) {
  ss.setTokens("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyLCJleHAiOjE3MDg4MzYwMjR9.eyDpdHrM2nFXCkyl6TH-mAh6IiPVbWyW2yGeUwk5Kq0", "two");
  }
  ngOnInit(): void {
    this.adamService.initialize();
  }

  ngOnDestroy(): void {
    this.adamService.destroy();
  }
}
