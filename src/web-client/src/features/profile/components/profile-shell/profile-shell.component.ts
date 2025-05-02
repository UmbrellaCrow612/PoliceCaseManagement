import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ProfileHeaderComponent } from "../profile-header/profile-header.component";

@Component({
  selector: 'app-profile-shell',
  imports: [RouterOutlet, ProfileHeaderComponent],
  templateUrl: './profile-shell.component.html',
  styleUrl: './profile-shell.component.css'
})
export class ProfileShellComponent {

}
