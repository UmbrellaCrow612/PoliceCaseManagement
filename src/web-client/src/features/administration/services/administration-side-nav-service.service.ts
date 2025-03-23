import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { CookieService } from '../../../core/browser/cookie/services/cookie.service';
import CookieNames from '../../../core/browser/cookie/constants/names';

@Injectable({
  providedIn: 'root',
})
export class AdministrationSideNavService {
  constructor(private cookieService: CookieService) {
    let value = this.getDefaultOpenState();
    this.isOpenSubject.next(value);
  }

  /**
   * Subscribe to this to listen to if the side bar nav is open and do stuff for changes ot state
   */
  isOpenSubject: BehaviorSubject<boolean> = new BehaviorSubject(false);

  /**
   * Subscribe to this to listen to if the side bar should be in mobile or is in mobile state is
   */
  isMobileSubject: BehaviorSubject<boolean> = new BehaviorSubject(false);

  /**
   * Open or close depending on current state of side bar and let subs know
   */
  toggle(bool: boolean) {
    this.cookieService.setCookie(
      CookieNames.ADMIN_SIDE_BAR_OPEN_PREF,
      JSON.stringify(bool)
    );
    this.isOpenSubject.next(bool);
  }

  /**
   * Change to mobile view - lets subs now side nav is mobile view now
   */
  toggleMobile(bool: boolean) {
    this.isMobileSubject.next(bool);
  }

  private getDefaultOpenState() {
    let prefCookie = this.cookieService.getCookie(
      CookieNames.ADMIN_SIDE_BAR_OPEN_PREF
    );
    if (!prefCookie) {
      this.cookieService.setCookie(
        CookieNames.ADMIN_SIDE_BAR_OPEN_PREF,
        JSON.stringify(false)
      );
    }

    let boolFlag = false;
    try {
      let parseValue = JSON.parse(prefCookie!);

      if (typeof parseValue === 'boolean') {
        boolFlag = parseValue;
      } else {
        throw new Error();
      }
    } catch (error) {
      console.error(
        `${CookieNames.ADMIN_SIDE_BAR_OPEN_PREF} cookie value cannot be parsed or is not a boolean value`
      );
      this.cookieService.setCookie(
        CookieNames.ADMIN_SIDE_BAR_OPEN_PREF,
        JSON.stringify(false)
      );
    }

    return boolFlag;
  }
}
