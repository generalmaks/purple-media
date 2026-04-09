import {inject, Injectable} from '@angular/core';
import {environment} from "../../../environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {AuthService} from "./auth.service";

export interface SendMessageDto {
  senderId: number,
  receiverId: number,
  content: string
}

export interface ChatMessage {
  id: number,
  senderId: number,
  receiverId: number,
  content: string,
  messageSent: string,
  isRead: boolean
}

export interface ChatInfo {
  otherUserId: number;
  otherUserUsername: string;
  lastMessageContent: string;
  lastMessageSentTime: string;
}

@Injectable({
  providedIn: 'root'
})
export class ChatMessageService {
  private apiUrl = environment.apiUrl + '/chatMessages'
  private http = inject(HttpClient)

  private auth = inject(AuthService)

  getMessageById(id: number) {
    return this.http.get(`${this.apiUrl}/${id}`)
  }

  getMessagesFromChatAsync(
    currentUserId: number,
    otherUserId: number,
    page: number,
    pageSize: number): Observable<ChatMessage[]> {
    return this.http.get<ChatMessage[]>(
      `${this.apiUrl}/thread/${currentUserId}/${otherUserId}/${page}/${pageSize}`
    )
  }

  getChatInfo(): Observable<ChatInfo[]> {
    return this.http.get<ChatInfo[]>(`${this.apiUrl}/chats-info`, {headers: this.getAuthHeaders()})
  }

  sendMessage(dto: SendMessageDto) {
    return this.http.post<ChatMessage>(`${this.apiUrl}`, dto)
  }

  deleteMessage(id: number
  ) {
    return this.http.delete(`${this.apiUrl}/${id}`)
  }

  private getAuthHeaders() {
    const token = this.auth.getToken()
    return {
      Authorization: `Bearer ${token}`
    };
  }
}
