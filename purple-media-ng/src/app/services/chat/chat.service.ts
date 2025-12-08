import {inject, Injectable} from '@angular/core';
import * as signalR from '@microsoft/signalr'
import {BehaviorSubject} from "rxjs";
import {environment} from "../../../environment";

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private hubConnection: signalR.HubConnection | undefined;

  public messages$ = new BehaviorSubject<{sender: string, text: string} | null>(null);

  constructor() { }

  public startConnection(token: string) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(environment.apiUrl.slice(0, -4) + '/chat', {
        accessTokenFactory: () => token
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err));

    this.hubConnection.on('ReceiveMessage', (senderId, message) => {
      this.messages$.next({ sender: senderId, text: message });
    });
  }

  public sendMessage(targetUserId: number, message: string
  ) {
    if (this.hubConnection) {
      this.hubConnection.invoke('SendPrivateMessage', targetUserId, message)
        .catch(err => console.error(err));
    }
  }
}
