import { Injectable } from '@angular/core';
import { AuthenticationService } from '../../authentication/services/authentication.service';
import { JwtService } from '../../authentication/services/jwt.service';

/**
 * AdamService is used to initialize application-wide logic or state when the app starts.
 *
 * @description Any initialization logic or state that needs to be executed on the creation of the app will be handled here.
 */

@Injectable({
  providedIn: 'root',
})
export class AdamService {
  constructor(private jwtService: JwtService) {}

  /**
   * Initializes the application-wide logic or state.
   */
  initialize() {
    this.jwtService.StartTokenValidationThroughoutLifetime();
  }

  /**
   * Destroy the application-wide logic or state.
   */
  destroy() {
    this.jwtService.DestroyAndStop();
  }
}
