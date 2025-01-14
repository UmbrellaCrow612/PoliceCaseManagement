import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-confirm-device-challenge-view',
  imports: [MatCardModule, MatButtonModule, MatButtonModule],
  templateUrl: './confirm-device-challenge-view.component.html',
  styleUrl: './confirm-device-challenge-view.component.css',
})
export class ConfirmDeviceChallengeViewComponent {}
