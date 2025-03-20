import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { appPaths } from '../../../../core/app/constants/appPaths';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-unauthorized-view',
  imports: [MatCardModule, MatButtonModule, MatIconModule, RouterLink],
  templateUrl: './unauthorized-view.component.html',
  styleUrl: './unauthorized-view.component.css',
})
export class UnauthorizedViewComponent {
  dashboardPageUrl = `/${appPaths.DASHBOARD}`;
}
