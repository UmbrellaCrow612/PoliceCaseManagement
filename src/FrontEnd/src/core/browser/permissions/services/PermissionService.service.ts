import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root',
  })
  export class PermissionService {
    constructor() {}
  
    // Request camera permission
    async requestCameraPermission(): Promise<boolean> {
      try {
        const stream = await navigator.mediaDevices.getUserMedia({ video: true });
        // Stop the stream immediately after getting permission
        stream.getTracks().forEach(track => track.stop());
        return true;
      } catch (error) {
        console.error('Camera permission error:', error);
        return false;
      }
    }
  
    // Request microphone permission
    async requestMicrophonePermission(): Promise<boolean> {
      try {
        const stream = await navigator.mediaDevices.getUserMedia({ audio: true });
        // Stop the stream immediately after getting permission
        stream.getTracks().forEach(track => track.stop());
        return true;
      } catch (error) {
        console.error('Microphone permission error:', error);
        return false;
      }
    }
  
    // Request location permission
    async requestLocationPermission(): Promise<boolean> {
      try {
        await new Promise((resolve, reject) => {
          navigator.geolocation.getCurrentPosition(resolve, reject);
        });
        return true;
      } catch (error) {
        console.error('Location permission error:', error);
        return false;
      }
    }
  
    // Check permission status
    async checkPermissionStatus(permissionName: PermissionName): Promise<PermissionState> {
      try {
        const result = await navigator.permissions.query({ name: permissionName });
        return result.state;
      } catch (error) {
        console.error(`Permission status check error for ${permissionName}:`, error);
        return 'denied';
      }
    }
  
    // Get current location if permission is granted
    async getLocation(): Promise<GeolocationPosition> {
      return new Promise((resolve, reject) => {
        if (!('geolocation' in navigator)) {
          reject(new Error('Geolocation not supported.'));
          return;
        }
  
        navigator.geolocation.getCurrentPosition(
          (position) => resolve(position),
          (error) => reject(error)
        );
      });
    }
  }