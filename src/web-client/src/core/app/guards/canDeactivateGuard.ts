import { CanDeactivateFn } from '@angular/router';
import { Observable } from 'rxjs';


/**
 * Use this on a class that is a view and has state that changes and you don't want to change views without losing that state 
 * unless a user confirms it
 */
export interface CanComponentDeactivate {

  /**
   * Method that returns a bool or observable that retuns a bool to indicate if a view can be changed from the current view
   * NOTE: make sure the route view uses @see canDeactivateGuard in routes  canDeactivate: [canDeactivateGuard]
   */
  canDeactivate(): boolean | Observable<boolean>;
}

/**
 * Use for components that have state change for form values and require saving before leaving
 */
export const canDeactivateGuard: CanDeactivateFn<CanComponentDeactivate> = (
  component
) => {
  return component.canDeactivate ? component.canDeactivate() : true;
};
