import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environment';

@Injectable({
  providedIn: 'root'
})
export class LikesService {
  private apiUrl = environment.apiUrl + '/Post';

  constructor(private http: HttpClient) { }

  likePost(id: number, username: string) {
    return this.http.put<any>(this.apiUrl + `/LikePost/${id}/${username}`, {})
  }

  getLikes(id: number) {
    return this.http.get<any>(this.apiUrl + `/likedBy/${id}`, {})
  }
}
