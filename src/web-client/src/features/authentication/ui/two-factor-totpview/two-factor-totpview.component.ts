import { Component } from '@angular/core';

@Component({
  selector: 'app-two-factor-totpview',
  imports: [],
  templateUrl: './two-factor-totpview.component.html',
  styleUrl: './two-factor-totpview.component.css'
})
export class TwoFactorTOTPViewComponent {
  qrCodeData: string | null = null;

  constructor() {}

  fetchQRCode(): void {
   
  }
}
