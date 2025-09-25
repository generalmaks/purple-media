import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {environment} from "../../environment";

@Injectable({
  providedIn: 'root'
})
export class PostService {
  private apiUrl = environment.apiUrl + '/Post';
  constructor(private http: HttpClient) { }

  postTweet(post: any){
    return this.http.post(this.apiUrl, post)
  }
}
