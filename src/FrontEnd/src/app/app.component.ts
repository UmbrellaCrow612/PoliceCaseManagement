import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { GetFingerPrint } from '../../utils/fingerprint';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = GetFingerPrint();
}
