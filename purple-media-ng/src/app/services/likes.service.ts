import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environment';

@Injectable({
  providedIn: 'root'
})
export class LikesService {
  private apiUrl = environment.apiUrl + '/Post/likedBy';

  constructor(private http: HttpClient) { }

  likePost(id: number, username: string) {
    return this.http.put<any>(this.apiUrl + `/${id}/${username}`, {})
  }

  getLikes(id: number) {
    return this.http.get<any>(this.apiUrl + `/${id}`, {})
  }
}
