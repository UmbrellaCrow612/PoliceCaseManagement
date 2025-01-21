import { Component } from '@angular/core';

@Component({
  selector: 'app-two-factor-totpview',
  imports: [],
  templateUrl: './two-factor-totpview.component.html',
  styleUrl: './two-factor-totpview.component.css'
})
export class TwoFactorTOTPViewComponent {
  qrCodeData: string | null = null;

  constructor(private http: HttpClient) {}

  fetchQRCode(): void {
    const url = 'http://localhost:5185/authentication/setup-totp'; // Replace with your endpoint
    this.http.get(url, { responseType: 'arraybuffer' }).subscribe(
      (response) => {
        // Convert byte array to Base64 string
        const base64String = btoa(
          String.fromCharCode(...new Uint8Array(response))
        );
        this.qrCodeData = `data:image/png;base64,${base64String}`;
      },
      (error) => {
        console.error('Error fetching QR code:', error);
      }
    );
  }
}
