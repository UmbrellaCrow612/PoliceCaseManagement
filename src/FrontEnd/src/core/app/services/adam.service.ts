import { Injectable } from '@angular/core';
import { AuthenticationService } from '../../authentication/services/authentication.service';

/**
 * AdamService is used to initialize application-wide logic or state when the app starts.
 *
 * @description Any initialization logic or state that needs to be executed on the creation of the app will be handled here.
 */

@Injectable({
  providedIn: 'root',
})
export class AdamService {
  constructor(private authService: AuthenticationService) {}

  /**
   * Initializes the application-wide logic or state.
   */
  initialize() {
    this.authService.StartTokenValidationThroughoutLifetime();
  }

  /**
   * Destroy the application-wide logic or state.
   */
  destroy(){
    this.authService.StopTokenValidation();
  }
}
