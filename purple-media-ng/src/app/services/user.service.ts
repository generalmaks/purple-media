import { HttpClient } from '@angular/common/http';
import {inject, Injectable} from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environment';

export enum UserRole {
  User = 0,
  Admin = 1
}

export interface User {
  id: number;
  username: string;
  displayName: string;
  bio?: string;
  profilePictureUrl?: string;
  userRole: UserRole;
  createdAt: Date;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/api/users`;

  get(userId: number): Observable<User | null> {
    return this.http.get<User | null>(`${this.apiUrl}/${userId}`);
  }

  getByUsername(username: string): Observable<User | null> {
    return this.http.get<User | null>(`${this.apiUrl}/by-username/${username}`);
  }

  create(username: string, displayName: string): Observable<User> {
    return this.http.post<User>(
      `${this.apiUrl}/${username}/${displayName}`,
      null
    );
  }

  update(userId: number, user: User): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${userId}`, user);
  }

  delete(userId: number): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl}/${userId}`);
  }
}
