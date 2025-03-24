import { Component } from '@angular/core';
import { CanComponentDeactivate } from '../../../../../core/app/guards/canDeactivateGuard';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-user-management-edit-user-view',
  imports: [],
  templateUrl: './user-management-edit-user-view.component.html',
  styleUrl: './user-management-edit-user-view.component.css',
})
export class UserManagementEditUserViewComponent
  implements CanComponentDeactivate
{
  hasUnsavedChanges = true;

  canDeactivate(): boolean | Observable<boolean> {
    if (this.hasUnsavedChanges) {
      return confirm('You have unsaved changes. Do you really want to leave?');
    }
    return true;
  }
}
