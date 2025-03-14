import { Injectable } from '@angular/core';
import { AuthenticationService } from '../../authentication/services/authentication.service';
import { JwtService } from '../../authentication/services/jwt.service';
import { DeviceService } from '../../user/device/services/device.service';

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
    private deviceService: DeviceService
  ) {}

  /**
   * Initializes the application-wide logic or state.
   */
  initialize() {
    this.jwtService.startTokenValidationThroughoutLifeTimeOfApp()
    this.deviceService.GetDeviceFingerPrint();
  }

  /**
   * Destroy the application-wide logic or state.
   */
  destroy() {
    this.jwtService.stopTokenValidationThroughoutLifeTimeOfApp()
  }
}
