import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {environment} from "../../environment";

@Injectable({
  providedIn: 'root'
})
export class FileServiceService {
  private apiUrl = environment.apiUrl + '/File';

  constructor(private http: HttpClient) { }

  getFile(id: number) {
    return this.http.get(`${this.apiUrl}/${id}`, { responseType: 'blob' });
  }
}
