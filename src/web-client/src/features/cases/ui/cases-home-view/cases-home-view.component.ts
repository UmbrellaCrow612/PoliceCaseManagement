import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-cases-home-view',
  imports: [MatButtonModule, RouterModule],
  templateUrl: './cases-home-view.component.html',
  styleUrl: './cases-home-view.component.css'
})
export class CasesHomeViewComponent {

}
