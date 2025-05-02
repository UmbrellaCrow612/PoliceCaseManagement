import { Component, inject } from '@angular/core';
import { UserService } from '../../../../core/user/services/user.service';

@Component({
  selector: 'app-profile-home-page',
  imports: [],
  templateUrl: './profile-home-page.component.html',
  styleUrl: './profile-home-page.component.css',
})
export class ProfileHomePageComponent {
  USER = inject(UserService).USER;
}
