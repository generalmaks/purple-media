import {inject, Injectable} from '@angular/core';
import {Observable} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environment";

@Injectable({
  providedIn: 'root'
})
export class LikeService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/api/like';

  like(userId: number, tweetId: number): Observable<boolean> {
    return this.http.post<boolean>(
      `${this.apiUrl}/like/${userId}/${tweetId}`,
      null
    );
  }

  unlike(userId: number, tweetId: number): Observable<boolean> {
    return this.http.post<boolean>(
      `${this.apiUrl}/unlike/${userId}/${tweetId}`,
      null
    );
  }

  isLiked(userId: number, tweetId: number): Observable<boolean> {
    return this.http.get<boolean>(
      `${this.apiUrl}/is-liked/${userId}/${tweetId}`
    );
  }

  countLikes(tweetId: number): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/count-likes/${tweetId}`);
  }
}
