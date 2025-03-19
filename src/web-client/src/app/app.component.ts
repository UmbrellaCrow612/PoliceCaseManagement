import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { JwtService } from '../core/authentication/services/jwt.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit, OnDestroy {
  constructor(private jwtService: JwtService) {}

  ngOnInit(): void {
    this.jwtService.startTokenValidationThroughoutLifeTimeOfApp();
    
  }

  ngOnDestroy(): void {
    this.jwtService.stopTokenValidation();
  }
}
