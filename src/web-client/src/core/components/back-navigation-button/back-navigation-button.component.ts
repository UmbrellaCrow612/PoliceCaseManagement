import { Component, input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ActivatedRoute, Router } from '@angular/router';

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
  constructor(private router: Router, private active: ActivatedRoute) {}

  /**
   * The url state of how much in the URL tree you want to go back, it has to be relative
   * @example
   *
   * Pass "../" will go back one segement in the ree url backwards
   * Pass "../../" if you want to go back two URL segements
   */
  navigationUrl = input.required<string>();

  onClicked() {
    this.router.navigate([this.navigationUrl()], { relativeTo: this.active });
  }
}
