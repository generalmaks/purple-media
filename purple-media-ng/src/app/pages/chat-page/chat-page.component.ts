import {Component, inject, OnInit} from '@angular/core';
import {ChatService} from "../../services/chat/chat.service";
import {ChatInfo, ChatMessage, ChatMessageService} from "../../services/http/chat-message.service";
import {AuthService} from "../../services/http/auth.service";
import {UserDto, UserService} from "../../services/http/user.service";
import {FormsModule} from "@angular/forms";
import {NgForOf, NgIf} from "@angular/common";

@Component({
  selector: 'app-chat-page',
  standalone: true,
  imports: [
    FormsModule,
    NgForOf,
    NgIf
  ],
  templateUrl: './chat-page.component.html',
  styleUrl: './chat-page.component.css'
})
export class ChatPageComponent implements OnInit {
  currentUser: UserDto
  chats: ChatInfo[]
  selectedChat: ChatInfo | null = null!

  selectedChatMessages: ChatMessage[]
  newMessage: string = ''

  pageSize = 10

  private chatHub = inject(ChatService)
  private chatMessages = inject(ChatMessageService)
  private authService = inject(AuthService)

  ngOnInit() {
    this.authService.me().subscribe({
      next: (userDto: UserDto) => this.currentUser = userDto,
      error: err => console.error('Could not determine user: ' + JSON.stringify(err))
    })

    this.loadChats()
    this.chatHub.startConnection(this.authService.getToken()!)
  }

  loadChats() {
    this.chatMessages.getChatInfo().subscribe({
      next: (chats: ChatInfo[]) => this.chats = chats,
      error: err => console.error('Could not log chats: ' + JSON.stringify(err))
    })
  }

  selectChat(chat: ChatInfo) {
    this.selectedChat = chat
    this.loadMessages(chat.otherUserId)
  }

  loadMessages(otherUserId: number) {
    this.chatMessages
      .getMessagesFromChatAsync(this.currentUser.id, otherUserId, 1, this.pageSize)
      .subscribe((msg: any) => {
        this.selectedChatMessages = msg.reverse()
      })
  }

  send() {
    if (!this.selectedChat || !this.newMessage.trim()) return;

    const otherUserId = this.selectedChat.otherUserId;
    const text = this.newMessage;

    this.chatHub.sendMessage(String(otherUserId), text);

    this.selectedChatMessages.push({
      content: this.newMessage, id: 0, isRead: false, messageSent: Date.now().toString(), receiverId: this.selectedChat.otherUserId, senderId: this.currentUser.id

    });

    this.newMessage = '';
  }
}
