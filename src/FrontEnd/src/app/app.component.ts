import { Component, OnDestroy, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AdamService } from '../core/app/services/adam.service';
import { CookieService } from '../core/browser/cookie/services/cookie.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit, OnDestroy {
  constructor(private adamService: AdamService, private cs : CookieService) {}

  ngOnInit(): void {
    this.adamService.initialize();
    this.cs.setCookie("id", "$$");
  }

  ngOnDestroy(): void {
    this.adamService.destroy();
  }
}
