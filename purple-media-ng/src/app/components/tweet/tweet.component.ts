import {Component, inject, Input, OnInit} from '@angular/core';
import {CommonModule, NgIf, NgOptimizedImage} from '@angular/common';
import {Router} from "@angular/router";
import {Tweet} from "../../services/http/tweet.service";
import {AttachmentService, TweetAttachment} from "../../services/http/attachment-service";
import {DomSanitizer} from "@angular/platform-browser";
import {environment} from "../../../environment";
import {User, UserService} from "../../services/http/user.service";
import {AuthService} from "../../services/http/auth.service";
import {FollowService} from "../../services/http/follow.service";

@Component({
  selector: 'app-tweet',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './tweet.component.html',
  styleUrl: './tweet.component.css'
})
export class TweetComponent implements OnInit{
  @Input() tweet: Tweet
  user: User
  attachments: TweetAttachment[] = []
  pfpAttachment: TweetAttachment
  apiUrl = environment.apiUrl
  isCurrentUserFollowingAuthorBool: boolean = false


  private router = inject(Router)
  private attachService = inject(AttachmentService)
  private userService = inject(UserService)
  private authService = inject(AuthService)
  private followService = inject(FollowService)


  ngOnInit() {
    this.attachService.getForTweet(this.tweet.id).subscribe({
      next: (atts) => {
        this.attachments = atts
        this.loadPfp()
      }, error : err => console.error("Failed at retreiving tweet attachments: " + err)
    })

    this.userService.get(this.tweet.authorId).subscribe({
      next: u => this.user = u!,
      error: err => console.error("Failed to retreive tweet user" + err)
    })

    this.isCurrentUserFollowingAuthor()
  }

  toUserProfile() {
    this.router.navigate([`/user/${this.tweet.authorId}`])
  }

  isImage(att: TweetAttachment): boolean {
    return att.mediaType.startsWith("image/")
  }

  loadPfp() {
    this.attachService.getForPfp(this.tweet.authorId).subscribe({
      next: (att) => {
        this.pfpAttachment = att
      }, error: err => console.error(err)
    })
  }

  isCurrentUserFollowingAuthor(){
    let currentUserId: number
    this.authService.me().subscribe({
      next: dto => {
        currentUserId = dto.id

        this.followService.isFollowing(currentUserId, this.user.id).subscribe({
          next: isFollowing => this.isCurrentUserFollowingAuthorBool = isFollowing,
          error: err => console.error('Could not determine if current user is following tweet author: ' + JSON.stringify(err))
        })
      },
      error: err => console.error('Could not determine current user from tweet: ' + JSON.stringify(err))
    })
  }
}
