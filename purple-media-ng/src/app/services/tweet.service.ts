import {inject, Injectable} from "@angular/core";
import {environment} from "../../environment";
import {Observable} from "rxjs";
import { HttpClient } from "@angular/common/http";

export interface Tweet {
  id: number;
  authorId: number;
  content: string;
  parentId?: number;
  createdAt: Date;
}


@Injectable({
  providedIn: 'root'
})
export class TweetService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/tweets`;

  getLatest(page: number, pageSize: number): Observable<Tweet[]> {
    return this.http.get<Tweet[]>(`${this.apiUrl}/latest/${page}/${pageSize}`)
  }

  get(tweetId: number): Observable<Tweet | null> {
    return this.http.get<Tweet | null>(`${this.apiUrl}/${tweetId}`);
  }

  getUserTweets(userId: number): Observable<Tweet[]> {
    return this.http.get<Tweet[]>(`${this.apiUrl}/from-user/${userId}`);
  }

  create(authorId: number, content: string, parentId?: number): Observable<Tweet> {
    const url = parentId
      ? `${this.apiUrl}/${authorId}/${content}/${parentId}`
      : `${this.apiUrl}/${authorId}/${content}`;
    return this.http.post<Tweet>(url, null);
  }

  delete(tweetId: number): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl}/${tweetId}`);
  }
}
