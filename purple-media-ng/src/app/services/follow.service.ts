import {inject, Injectable} from '@angular/core';
import {environment} from "../../environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class FollowService {
  private apiUrl = environment.apiUrl + '/follow';
  private http = inject(HttpClient)

  follow(followerId: number, followingId: number): Observable<boolean> {
    return this.http.post<boolean>(
      `${this.apiUrl}/follow/${followerId}/${followingId}`,
      null
    );
  }

  unfollow(followerId: number, followingId: number): Observable<boolean> {
    return this.http.post<boolean>(
      `${this.apiUrl}/unfollow/${followerId}/${followingId}`,
      null
    );
  }

  isFollowing(followerId: number, followingId: number): Observable<boolean> {
    return this.http.get<boolean>(
      `${this.apiUrl}/following/${followerId}/${followingId}`
    );
  }

  countFollowers(userId: number): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/count-followers/${userId}`);
  }

  countFollowing(userId: number): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/count-following/${userId}`);
  }
}
