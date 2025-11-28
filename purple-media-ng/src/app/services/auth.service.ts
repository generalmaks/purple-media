import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from "../../environment";
import {jwtDecode} from 'jwt-decode';
import {catchError, Observable, tap, throwError} from "rxjs";

export interface RegisterDto {
  username: string;
  displayName: string;
  unhashedPassword: string;
}

export interface LoginDto {
  username: string,
  unhashedPassword: string
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = environment.apiUrl + '/auth'
  private http = inject(HttpClient)
  private tokenKey = 'jwt-token';

  register(dto: RegisterDto): Observable<void> {
    return this.http.post<void>(
      `${this.apiUrl}/register/${dto.username}/${dto.displayName}/${dto.unhashedPassword}`,
      null);
  }

  login(dto: LoginDto): Observable<string> {
    return this.http.post(
      `${this.apiUrl}/login/${dto.username}/${dto.unhashedPassword}`,
      dto,
      { responseType: 'text'}
    ).pipe(
      tap(token => localStorage.setItem(this.tokenKey, token)),
      catchError(err => {
        console.error('Login error: ' + err)
        return throwError(() => err)
      })
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
