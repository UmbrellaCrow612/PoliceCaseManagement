import { Component, input, model, OnInit, output } from '@angular/core';
import { UserRoles } from '../../../../../../../core/authentication/roles';
import { MatListModule, MatSelectionListChange } from '@angular/material/list';

@Component({
  selector: 'app-user-management-edit-user-roles-view',
  imports: [MatListModule],
  templateUrl: './user-management-edit-user-roles-view.component.html',
  styleUrl: './user-management-edit-user-roles-view.component.css',
})
export class UserManagementEditUserRolesViewComponent {
  roles = model.required<string[]>();
  allSystemRoles: string[] = UserRoles.all();

  /**
   * Emits if there was a change to the roles
   */
  selectionChangedEvent = output<void>();

  onSelectionChange(event: MatSelectionListChange) {
    this.selectionChangedEvent.emit();
    this.roles.update(() => {
      return event.source.selectedOptions.selected.map(
        (option) => option.value
      );
    });
  }
}
