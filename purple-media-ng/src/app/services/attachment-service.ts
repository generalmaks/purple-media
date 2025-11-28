import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from "../../environment";
import {Observable} from "rxjs";

export interface TweetAttachment {
  id: number;
  tweetId: number;
  url: string;
  contentType: string;
}

@Injectable({
  providedIn: 'root'
})
export class AttachmentService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/api/attachments`;

  create(tweetId: number, url: string, contentType: string): Observable<void> {
    return this.http.post<void>(this.apiUrl, null, {
      params: {tweetId, url, contentType}
    });
  }

  getForTweet(tweetId: number): Observable<TweetAttachment[]> {
    return this.http.get<TweetAttachment[]>(`${this.apiUrl}/${tweetId}`);
  }
}
