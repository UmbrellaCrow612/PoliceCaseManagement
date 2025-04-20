import { Component } from '@angular/core';
import { CasesHeaderComponent } from "../cases-header/cases-header.component";
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-cases-shell',
  imports: [CasesHeaderComponent, RouterModule],
  templateUrl: './cases-shell.component.html',
  styleUrl: './cases-shell.component.css'
})
export class CasesShellComponent {

}
