import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environment';
import {SearchResult} from "../interfaces/search-result";
import {Tweet} from "../interfaces/tweet";

@Injectable({
  providedIn: 'root'
})
export class TweetService {
  private apiUrl = environment.apiUrl + '/Post';

  constructor(private http: HttpClient) { }

  getTweets(): Observable<any[]> {
    return this.http.get<Tweet[]>(this.apiUrl)
  }

  getTweetsByUser(userId: string) {
    return this.http.get<Tweet[]>(this.apiUrl + '/GetByUsername/' + userId)
  }

  searchTweets(snippet: string){
    return this.http.get<SearchResult[]>(this.apiUrl + '/search/'+snippet)
  }

  postTweet(post: any){
    return this.http.post<Tweet>(this.apiUrl, post)
  }

  getTweetsById(id: string) {
    return this.http.get<Tweet>(this.apiUrl + '/' + id);
  }

  getResponsesToTweet(id: string) {
    return this.http.get<Tweet[]>(this.apiUrl + '/responses/' + id);
  }
}
