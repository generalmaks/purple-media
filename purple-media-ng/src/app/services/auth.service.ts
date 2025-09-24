import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import {environment} from "../../environment";
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = environment.apiUrl + '/Auth'
  private tokenKey = 'jwt-token';

  constructor(private http: HttpClient) { }

  register(username: string, password: string, email: string) {
    return this.http.post(`${this.apiUrl}/register`, { username, password, email});
  }

  login(username: string, password: string) {
    return this.http.post<{ token: string }>(`${this.apiUrl}/login`, { username, password })
      .pipe(tap(response => {
        localStorage.setItem(this.tokenKey, response.token);
      }));
  }

  logout() {
    localStorage.removeItem(this.tokenKey);
  }

  getToken() {
    return localStorage.getItem(this.tokenKey);
  }

  isLoggedIn() {
    return !!this.getToken();
  }

  getUsername(): string | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      const decoded: any = jwtDecode(token)
      return decoded.username || decoded.sub
    } catch (e) {
      console.error('Invalid JWT: ', e);
      return null;
    }
  }
}
