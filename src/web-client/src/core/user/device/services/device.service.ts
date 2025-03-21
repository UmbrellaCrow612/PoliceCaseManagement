import { Injectable } from '@angular/core';
import { ComputeFingerPrint } from '../utils/FingerPrint';
import { CookieService } from '../../../browser/cookie/services/cookie.service';
import CookieNames from '../../../browser/cookie/constants/names';
import { HttpClient } from '@angular/common/http';
import env from '../../../../environments/environment';
import {
  SendDeviceChallengeAttemptRequestBody,
  ValidateDeviceChallengeCode,
} from '../type';

@Injectable({
  providedIn: 'root',
})
export class DeviceService {
  private readonly COOKIE_NAME = CookieNames.DEVICE_FINGERPRINT;
  private readonly EXPIRATION_DAYS = 2;

  private readonly BASE_URL = env.BaseUrls.authenticationBaseUrl;

  constructor(private http: HttpClient, private cookieService: CookieService) {}

  SendDeviceChallengeAttempt(body: SendDeviceChallengeAttemptRequestBody) {
    return this.http.post(
      `${this.BASE_URL}/authentication/send-user-device-challenge`,
      body
    );
  }

  ValidateDeviceChallengeCode(body: ValidateDeviceChallengeCode) {
    return this.http.post(
      `${this.BASE_URL}/authentication/validate-user-device-challenge`,
      body
    );
  }

  /**
   * Get the device fingerprint. Either retrieve it from cookies or compute a new one.
   * @returns The device fingerprint.
   */
  GetDeviceFingerPrint(): string {
    const existingCookie = this.cookieService.getCookie(this.COOKIE_NAME);

    if (existingCookie) {
      return existingCookie;
    }

    let fingerPrint = ComputeFingerPrint();
    const oneWeekFromNow = new Date();
    oneWeekFromNow.setDate(oneWeekFromNow.getDate() + 7);

    this.cookieService.setCookie(this.COOKIE_NAME, fingerPrint, {
      Expires: oneWeekFromNow,
    });

    return fingerPrint;
  }
}
