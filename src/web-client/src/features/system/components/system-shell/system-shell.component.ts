import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SystemHeaderComponent } from "../system-header/system-header.component";

@Component({
  selector: 'app-system-shell',
  imports: [RouterModule, SystemHeaderComponent],
  templateUrl: './system-shell.component.html',
  styleUrl: './system-shell.component.css'
})
export class SystemShellComponent {

}
