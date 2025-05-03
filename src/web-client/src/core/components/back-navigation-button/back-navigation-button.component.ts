import { Location } from '@angular/common';
import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-back-navigation-button',
  imports: [MatButtonModule, MatIconModule],
  templateUrl: './back-navigation-button.component.html',
  styleUrl: './back-navigation-button.component.css',
})
/**
 * Used to navigate backwards in the browser URL history
 */
export class BackNavigationButtonComponent {
  constructor(private location: Location) {}
  onClicked() {
    this.location.back();
  }
}
