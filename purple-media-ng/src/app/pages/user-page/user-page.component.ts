import {Component, inject, OnInit} from '@angular/core';
import {User, UserService} from '../../services/http/user.service';
import {DatePipe} from '@angular/common';
import {ActivatedRoute} from "@angular/router";
import {Tweet, TweetService} from "../../services/http/tweet.service";
import {TweetComponent} from "../../components/tweet/tweet.component";
import {environment} from "../../../environment";
import {AttachmentService, TweetAttachment} from "../../services/http/attachment-service";
import {FollowService} from "../../services/http/follow.service";
import {AuthService} from "../../services/http/auth.service";

@Component({
  selector: 'app-user-page',
  standalone: true,
  imports: [DatePipe, TweetComponent],
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

  private userService = inject(UserService)
  private activatedRoute = inject(ActivatedRoute)
  private tweetService = inject(TweetService)
  private attachmentService = inject(AttachmentService)
  private authService = inject(AuthService)
  private followService = inject(FollowService)

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
    if(this.isCurrentUserFollowing){
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
