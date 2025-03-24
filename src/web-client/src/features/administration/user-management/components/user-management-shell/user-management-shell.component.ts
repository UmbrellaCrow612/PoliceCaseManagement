import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { UserManagementHeaderComponent } from "../user-management-header/user-management-header.component";

@Component({
  selector: 'app-user-management-shell',
  imports: [RouterModule, UserManagementHeaderComponent],
  templateUrl: './user-management-shell.component.html',
  styleUrl: './user-management-shell.component.css',
})
export class UserManagementShellComponent {
  
}
