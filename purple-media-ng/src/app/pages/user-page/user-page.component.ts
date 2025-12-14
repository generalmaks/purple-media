import {Component, inject, OnInit} from '@angular/core';
import {User, UserDto, UserService} from '../../services/http/user.service';
import {DatePipe} from '@angular/common';
import {ActivatedRoute, Router} from "@angular/router";
import {Tweet, TweetService} from "../../services/http/tweet.service";
import {TweetComponent} from "../../components/tweet/tweet.component";
import {environment} from "../../../environment";
import {AttachmentService, TweetAttachment} from "../../services/http/attachment-service";
import {FollowService} from "../../services/http/follow.service";
import {AuthService} from "../../services/http/auth.service";
import {ChatMessageService, SendMessageDto} from "../../services/http/chat-message.service";
import {FormsModule} from "@angular/forms";

@Component({
  selector: 'app-user-page',
  standalone: true,
  imports: [DatePipe, TweetComponent, FormsModule],
  templateUrl: './user-page.component.html',
  styleUrl: './user-page.component.css'
})
export class UserPageComponent implements OnInit {
  userId: number
  user: User
  userTweets: Tweet[] = []
  apiUrl = environment.apiUrl;
  pfpAttachment: TweetAttachment | null;
  isCurrentUserFollowing: boolean
  currentUserId: number
  alreadyHasChat: boolean = true
  firstMessageContent: string = ''
  private userService = inject(UserService)
  private activatedRoute = inject(ActivatedRoute)
  private tweetService = inject(TweetService)
  private attachmentService = inject(AttachmentService)
  private authService = inject(AuthService)
  private followService = inject(FollowService)
  private messageService = inject(ChatMessageService)
  private router = inject(Router)

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.userId = Number(params.get("id"))

      this.loadUser()
      this.loadUserTweets()
    })

    this.authService.me().subscribe({
      next: currentUser => {
        this.currentUserId = currentUser.id

        this.followService.isFollowing(this.currentUserId, this.userId).subscribe({
          next: isFollowing => this.isCurrentUserFollowing = isFollowing,
          error: err => console.error('Could not determine if current user is following: ' + JSON.stringify(err))
        })
      },
      error: err => console.error('Could not get current users ID: ' + JSON.stringify(err))
    })
  }

  follow() {
    if (this.isCurrentUserFollowing) {
      this.followService.unfollow(this.currentUserId, this.userId).subscribe({
        next: () => this.isCurrentUserFollowing = false,
        error: err => console.error('Could not unfollow user: ' + JSON.stringify(err))
      })
    } else {
      this.followService.follow(this.currentUserId, this.userId).subscribe({
        next: () => this.isCurrentUserFollowing = true,
        error: err => console.error('Could not follow user: ' + JSON.stringify(err))
      })
    }
  }

  sendMessage() {
    if (!this.currentUserId) return

    this.messageService.getMessagesFromChatAsync(this.currentUserId, this.userId, 1, 1).subscribe({
      next: lastMessage => {
        if (lastMessage.toString() == '') {
          this.alreadyHasChat = false
        } else {
          this.alreadyHasChat = true
          this.router.navigate(['/chat'])
        }
      }, error: err => console.error('Could not get last message: ' + JSON.stringify(err))
    })
  }

  sendFirstMessage() {
    let sendMessageDto: SendMessageDto = {
      senderId: this.currentUserId,
      receiverId: this.userId,
      content: this.firstMessageContent
    }
    this.messageService.sendMessage(sendMessageDto).subscribe({
      next: () => this.router.navigate(['/chat']),
      error: err => console.error('Could not send first message: ' + JSON.stringify(err))
    })
  }

  private loadUser() {
    this.userService.get(this.userId).subscribe(user => {
      this.user = user!
      this.loadPfp()
    })
  }

  private loadUserTweets() {
    this.tweetService.getUserTweets(this.userId).subscribe(tweets => {
      this.userTweets = tweets
    })
  }

  private loadPfp() {
    this.attachmentService.getForPfp(this.userId).subscribe({
      next: (att) => {
        this.pfpAttachment = att
      }, error: err => this.pfpAttachment = null
    })
  }
}
