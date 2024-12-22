import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { PermissionService } from '../core/browser/permissions/services/PermissionService.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {


  constructor(private permissionService : PermissionService) {}

  async requestCamera(): Promise<void> {
    try {
      const granted = await this.permissionService.requestCameraPermission();
      if (granted) {
        console.log('Camera permission granted!');
        // Proceed with camera-related functionality
      } else {
        console.log('Camera permission denied.');
      }
    } catch (error) {
      console.error('Error requesting camera permission:', error);
    }
  }

  async requestMicrophone(): Promise<void> {
    try {
      const granted = await this.permissionService.requestMicrophonePermission();
      if (granted) {
        console.log('Microphone permission granted!');
        // Proceed with microphone-related functionality
      } else {
        console.log('Microphone permission denied.');
      }
    } catch (error) {
      console.error('Error requesting microphone permission:', error);
    }
  }

  async requestLocation(): Promise<void> {
    try {
      const granted = await this.permissionService.requestLocationPermission();
      if (granted) {
        console.log('Location permission granted!');
        // Proceed with location-related functionality
      } else {
        console.log('Location permission denied.');
      }
    } catch (error) {
      console.error('Error requesting location permission:', error);
    }
  }

  async getLocationData(): Promise<void> {
    try {
      const position = await this.permissionService.getLocation();
      console.log('Current position:', {
        latitude: position.coords.latitude,
        longitude: position.coords.longitude
      });
    } catch (error) {
      console.error('Error getting location:', error);
    }
  }
  
}
