import { Injectable } from '@angular/core';
import { ComputeFingerPrint } from '../utils/FingerPrint';
import { CookieService } from '../../../browser/cookie/services/cookie.service';
import CookieNames from '../../../browser/cookie/constants/names';
import { BaseService } from '../../../http/services/BaseService.service';
import { HttpClient } from '@angular/common/http';
import env from '../../../../environments/environment';
import {
  SendDeviceChallengeAttemptRequestBody,
  ValidateDeviceChallengeCode,
} from '../type';

@Injectable({
  providedIn: 'root',
})
export class DeviceService extends BaseService {
  private readonly COOKIE_NAME = CookieNames.DEVICE_FINGERPRINT;
  private readonly EXPIRATION_DAYS = 2;

  private readonly BASE_URL = env.BaseUrls.authenticationBaseUrl;

  constructor(
    protected override http: HttpClient,
    private cookieService: CookieService
  ) {
    super(http);
  }

  SendDeviceChallengeAttempt(body: SendDeviceChallengeAttemptRequestBody) {
    return this.post(
      `${this.BASE_URL}/authentication/send-user-device-challenge`,
      body
    );
  }

  ValidateDeviceChallengeCode(body: ValidateDeviceChallengeCode) {
    return this.post(
      `${this.BASE_URL}/authentication/validate-user-device-challenge`,
      body
    );
  }

  /**
   * Get the device fingerprint. Either retrieve it from cookies or compute a new one.
   * @returns The device fingerprint.
   */
  GetDeviceFingerPrint(): string {
    try {
      const existingCookie = this.cookieService.getCookie(this.COOKIE_NAME);

      if (existingCookie) {
        console.log('Existing cookie found for device fingerprint');
        const { fingerprint, timestamp } = JSON.parse(existingCookie);
        const daysSinceCreation = this.getDaysSinceTimestamp(timestamp);

        if (daysSinceCreation < this.EXPIRATION_DAYS) {
          return fingerprint;
        }
      }

      return this.computeAndStoreFingerprint();
    } catch {
      return this.computeAndStoreFingerprint();
    }
  }

  /**
   * Compute a new fingerprint, store it in a cookie, and return it.
   * @returns The newly computed device fingerprint.
   */
  private computeAndStoreFingerprint(): string {
    try {
      const fingerprint = ComputeFingerPrint();
      const cookieValue = JSON.stringify({
        fingerprint,
        timestamp: new Date().getTime(),
      });

      this.cookieService.setCookie(this.COOKIE_NAME, cookieValue);

      return fingerprint;
    } catch {
      return ComputeFingerPrint();
    }
  }

  /**
   * Calculate the number of days since a given timestamp.
   * @param timestamp - The timestamp to calculate from.
   * @returns The number of days since the timestamp.
   */
  private getDaysSinceTimestamp(timestamp: number): number {
    const now = new Date().getTime();
    const millisecondsDiff = now - timestamp;
    return millisecondsDiff / (1000 * 60 * 60 * 24);
  }
}
