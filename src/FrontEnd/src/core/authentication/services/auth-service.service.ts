import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { jwtDecode } from 'jwt-decode';

interface JwtPayload {
  sub: string;
  exp: number;
  roles?: string[];
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<any>;
  public currentUser: Observable<any>;

  constructor(private http: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<any>(
      JSON.parse(localStorage.getItem('currentUser') || 'null')
    );
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue() {
    return this.currentUserSubject.value;
  }

  login(username: string, password: string) {
    return this.http.post<any>('/api/authenticate', { username, password })
      .pipe(map(response => {
        // Login successful if there's a jwt token in the response
        if (response && response.token) {
          // Store user details and jwt token in local storage
          localStorage.setItem('currentUser', JSON.stringify(response));
          this.currentUserSubject.next(response);
        }
        return response;
      }));
  }

  logout() {
    // Remove user from local storage
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }

  isTokenExpired(): boolean {
    const token = this.getCurrentToken();
    if (!token) return true;

    try {
      const decoded = jwtDecode<JwtPayload>(token);
      const currentTime = Date.now() / 1000;
      return decoded.exp < currentTime;
    } catch (error) {
      return true;
    }
  }

  getCurrentToken(): string | null {
    const currentUser = JSON.parse(localStorage.getItem('currentUser') || 'null');
    return currentUser ? currentUser.token : null;
  }

  getUserRoles(): string[] {
    const token = this.getCurrentToken();
    if (!token) return [];

    try {
      const decoded = jwtDecode<JwtPayload>(token);
      return decoded.roles || [];
    } catch (error) {
      return [];
    }
  }

  refreshToken() {
    return this.http.post<any>('/api/refresh-token', {})
      .pipe(map(response => {
        if (response && response.token) {
          const currentUser = JSON.parse(localStorage.getItem('currentUser') || 'null');
          currentUser.token = response.token;
          localStorage.setItem('currentUser', JSON.stringify(currentUser));
          this.currentUserSubject.next(currentUser);
        }
        return response;
      }));
  }
}