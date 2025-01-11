import { Component, OnDestroy, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AdamService } from '../core/app/services/adam.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit, OnDestroy {
  constructor(private adamService: AdamService) {}

  ngOnInit(): void {
    this.adamService.initialize();
  }

  ngOnDestroy(): void {
    this.adamService.destroy();
  }
}
