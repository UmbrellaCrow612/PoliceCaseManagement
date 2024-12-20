import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { DeviceService } from '../core/device/services/device.service';
import { AuthenticationService } from '../core/authentication/services/authentication.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  value: string = '';

  constructor(private deviceService: DeviceService, private authService: AuthenticationService) {}

  ngOnInit() {
    try {
      this.value = this.deviceService.GetDeviceFingerPrint();
      this.authService.SetJwtToken(this.value, 60);
    } catch (error) {
      console.error('Error getting device fingerprint:', error);
    }
  }
}
