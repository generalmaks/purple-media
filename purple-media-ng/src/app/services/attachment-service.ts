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

  create(tweetId: number, file: File): Observable<TweetAttachment> {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('tweetId', tweetId.toString());

    return this.http.post<TweetAttachment>(this.apiUrl, formData);
  }

  getForTweet(tweetId: number): Observable<TweetAttachment[]> {
    return this.http.get<TweetAttachment[]>(`${this.apiUrl}/${tweetId}`);
  }

  delete(attachmentId: number): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl}/${attachmentId}`);
  }
}
