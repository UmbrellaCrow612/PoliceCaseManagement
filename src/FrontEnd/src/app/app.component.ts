import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MediaDevicesService } from '../core/browser/media/services/mediaDevices.service';
import { WebGLService } from '../core/browser/webGL/services/webGL.service';
import { ScreenService } from '../core/browser/window/services/screen.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  val:any = null
  constructor(private ss : ScreenService){
    this.val = ss.getScreenInfo()
  }
}
