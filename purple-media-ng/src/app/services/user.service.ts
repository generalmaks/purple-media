import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'http://localhost:5101/api/User/'

  constructor(private http: HttpClient) { }

  getTweets(id: string): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl + id)
  }
}
