import { Component } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav';

@Component({
  selector: 'app-administration-shell',
  imports: [MatSidenavModule],
  templateUrl: './administration-shell.component.html',
  styleUrl: './administration-shell.component.css',
})
export class AdministrationShellComponent {}
