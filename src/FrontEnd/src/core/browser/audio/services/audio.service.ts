import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AudioService {
  constructor() {}

  /**
   * Get audio-related information about the device and browser.
   * This method retrieves information like available audio input/output devices
   * and other relevant browser details for audio capabilities.
   *
   * @returns A promise that resolves with an object containing audio information.
   */
  async getAudioDeviceInfo(): Promise<{
    inputDevices: MediaDeviceInfo[];
    outputDevices: MediaDeviceInfo[];
    browserSupportsAudioContext: boolean;
  }> {
    try {
      // Check if the browser supports AudioContext
      const browserSupportsAudioContext = 'AudioContext' in window || 'webkitAudioContext' in window;

      // Get a list of media devices
      const devices = await navigator.mediaDevices.enumerateDevices();

      // Filter for audio input and output devices
      const inputDevices = devices.filter(device => device.kind === 'audioinput');
      const outputDevices = devices.filter(device => device.kind === 'audiooutput');

      return {
        inputDevices,
        outputDevices,
        browserSupportsAudioContext,
      };
    } catch (error) {
      console.error('Error getting audio device information:', error);
      return {
        inputDevices: [],
        outputDevices: [],
        browserSupportsAudioContext: false,
      };
    }
  }
}
