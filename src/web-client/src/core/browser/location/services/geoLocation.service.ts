import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class GeoLocationService {
  
  getUserGeolocation(): Promise<GeolocationPosition> {
    return new Promise((resolve, reject) => {
      if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
          (position: GeolocationPosition) => {
            resolve(position);
          },
          (error: GeolocationPositionError) => {
            reject(error);
          }
        );
      } else {
        reject('Geolocation is not supported by this browser.');
      }
    });
  }
}
