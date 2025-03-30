import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TweetService {
  private apiUrl = 'http://localhost:5101/api/Post/'

  constructor(private http: HttpClient) { }

  getTweets(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl)
  }

  getTweetsByUser(userId: string) {
    return this.http.get<any[]>(this.apiUrl + 'GetByUsername/' + userId)
  }
}
