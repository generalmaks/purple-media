import {Component, inject, OnInit} from '@angular/core';
import {ChatService} from "../../services/chat/chat.service";
import {ChatInfo, ChatMessage, ChatMessageService} from "../../services/http/chat-message.service";
import {AuthService} from "../../services/http/auth.service";
import {UserDto} from "../../services/http/user.service";
import {FormsModule} from "@angular/forms";
import {CommonModule, NgForOf, NgIf} from "@angular/common";

@Component({
  selector: 'app-chat-page',
  standalone: true,
  imports: [
    FormsModule,
    NgForOf,
    NgIf,
    CommonModule
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

  readonly pageSize = 10

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

    this.chatHub.messages$.subscribe(msg => {
      if (!msg) return

      if (this.selectedChat && this.selectedChat.otherUserId === Number(msg.sender)) {
        this.selectedChatMessages.push({
          senderId: Number(msg.sender),
          receiverId: this.currentUser.id,
          content: msg.text,
          isRead: false,
          id: 0,
          messageSent: new Date().toISOString()
        });
      }
    })
  }

  selectChat(chat: ChatInfo) {
    this.selectedChat = chat
    this.loadMessages(chat.otherUserId)
  }

  send() {
    if (!this.selectedChat || !this.newMessage.trim()) return;

    const otherUserId = this.selectedChat.otherUserId;
    const text = this.newMessage;

    this.chatHub.sendMessage(otherUserId, text);

    this.selectedChatMessages.push({
      content: this.newMessage,
      id: 0,
      isRead: false,
      messageSent: Date.now().toString(),
      receiverId: this.selectedChat.otherUserId,
      senderId: this.currentUser.id

    });

    this.newMessage = '';
  }

  closeChat() {
    this.selectedChat = null;
    this.selectedChatMessages = [];
  }

  private loadChats() {
    this.chatMessages.getChatInfo().subscribe({
      next: (chats: ChatInfo[]) => this.chats = chats,
      error: err => console.error('Could not log chats: ' + JSON.stringify(err))
    })
  }

  private loadMessages(otherUserId: number) {
    this.chatMessages
      .getMessagesFromChatAsync(this.currentUser.id, otherUserId, 1, this.pageSize)
      .subscribe((msg: any) => {
        this.selectedChatMessages = msg.reverse()
      })
  }
}
