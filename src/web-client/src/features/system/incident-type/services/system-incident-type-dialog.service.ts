import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
/**
 * Service used to listen to if a user clicked delete and it was successful so you can do some extra actions
 */
export class SystemIncidentTypeDialogService {
  constructor() {}

  /**
   * Subscribe to see if a user clicked delete in the dialog and it was successful so you can do some extra work
   */
  DELETEDSubject = new Subject<boolean>();
}
