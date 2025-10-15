import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = environment.apiUrl + '/User/'

  constructor(private http: HttpClient) { }

  getUserPublicInfo(id: string) {
    return this.http.get(`${this.apiUrl}${id}`)
  }
}
