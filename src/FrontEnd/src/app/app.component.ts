import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.checkAuthorization();
  }

  checkAuthorization(): void {
    const apiUrl = 'https://localhost:7058/authorization/check';

    this.http.get(apiUrl)
      .subscribe({
        next: (response) => {
          console.log('Authorization Check Response:', response);
        },
        error: (error) => {
          console.error('Error occurred while checking authorization:', error);
        }
      });
  }
}
