import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from "../../environment";
import {jwtDecode} from 'jwt-decode';
import {Observable} from "rxjs";

export interface RegisterDto {
  username: string;
  displayName: string;
  unhashedPassword: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = environment.apiUrl + '/Auth'
  private http = inject(HttpClient)
  private tokenKey = 'jwt-token';

  register(username: string, displayName: string, unhashedPassword: string): Observable<void> {
    return this.http.post<void>(
      `${this.apiUrl}/register/${username}/${displayName}/${unhashedPassword}`,
      null);
  }

  login(username: string, unhashedPassword: string): Observable<string> {
    return this.http.post<string>(
      `${this.apiUrl}/login/${username}/${unhashedPassword}`,
      null
    )
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
