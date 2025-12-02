import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from "../../environment";
import {Observable} from "rxjs";

export interface TweetAttachment {
  id: number;
  name: string;
  mediaType: string;
  url: string
}

@Injectable({
  providedIn: 'root'
})
export class AttachmentService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/attachments`;

  create(tweetId: number, file: File): Observable<TweetAttachment> {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('tweetId', tweetId.toString());

    return this.http.post<TweetAttachment>(this.apiUrl, formData);
  }

  getForTweet(tweetId: number): Observable<TweetAttachment[]> {
    return this.http.get<TweetAttachment[]>(`${this.apiUrl}/${tweetId}`);
  }

  getForPfp(userId: number): Observable<TweetAttachment> {
    return this.http.get<TweetAttachment>(`${this.apiUrl}/pfp/${userId}`)
  }
}
