import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { DeviceService } from '../core/device/services/device.service';
import { AuthenticationService } from '../core/authentication/services/authentication.service';
import { EncryptionService } from '../core/encryption/services/encryption.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  value: string = '';

  constructor(private deviceService: DeviceService, private authService: AuthenticationService, private es : EncryptionService) {}

  async ngOnInit() {
    try {
      this.value = await this.es.Hash("Test");
      this.authService.SetJwtToken(this.value, 60);
    } catch (error) {
      console.error('Error getting device fingerprint:', error);
    }
  }
}
