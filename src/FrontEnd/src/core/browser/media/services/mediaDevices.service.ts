import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class MediaDevicesService {

  constructor() { }

  getMediaDevices(): Promise<MediaDeviceInfo[]> {
    return new Promise((resolve, reject) => {
      if (navigator.mediaDevices && navigator.mediaDevices.enumerateDevices) {
        navigator.mediaDevices.enumerateDevices()
          .then(devices => {
            resolve(devices);
          })
          .catch(error => {
            reject('Error retrieving media devices: ' + error);
          });
      } else {
        reject('MediaDevices API not supported by this browser.');
      }
    });
  }

  getMediaStream(deviceId: string, mediaType: 'audio' | 'video' = 'audio'): Promise<MediaStream> {
    const constraints: MediaStreamConstraints = {};
    
    if (mediaType === 'audio') {
      constraints.audio = { deviceId: { exact: deviceId } };
    } else if (mediaType === 'video') {
      constraints.video = { deviceId: { exact: deviceId } };
    }
    
    return navigator.mediaDevices.getUserMedia(constraints);
  }
}
