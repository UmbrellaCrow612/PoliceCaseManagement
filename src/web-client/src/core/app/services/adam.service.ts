import { Injectable } from '@angular/core';
import { JwtService } from '../../authentication/services/jwt.service';
import { UserService } from '../../user/services/user.service';

/**
 * AdamService is used to initialize application-wide logic or state when the app starts.
 *
 * @description Any initialization logic or state that needs to be executed on the creation of the app will be handled here.
 */

@Injectable({
  providedIn: 'root',
})
export class AdamService {
  constructor(
    private jwtService: JwtService,
    private userService: UserService
  ) {}

  /**
   * Initializes the application-wide logic or state.
   */
  initialize() {
    this.jwtService.startTokenValidationThroughoutLifeTimeOfApp()
    this.userService.setCurrentUser()
  }

  /**
   * Destroy the application-wide logic or state.
   */
  destroy() {
    this.jwtService.stopTokenValidationThroughoutLifeTimeOfApp()
  }
}
