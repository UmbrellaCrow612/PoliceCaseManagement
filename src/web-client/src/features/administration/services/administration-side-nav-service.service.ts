import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AdministrationSideNavService {
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
    this.isOpenSubject.next(bool);
  }

  /**
   * Change to mobile view - lets subs now side nav is mobile view now
   */
  toggleMobile(bool: boolean) {
    this.isMobileSubject.next(bool);
  }
}
