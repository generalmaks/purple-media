import {Component, inject, OnInit} from '@angular/core';
import {User, UserService} from '../../services/http/user.service';
import {DatePipe} from '@angular/common';
import {ActivatedRoute} from "@angular/router";
import {Tweet, TweetService} from "../../services/http/tweet.service";
import {TweetComponent} from "../../components/tweet/tweet.component";
import {environment} from "../../../environment";
import {AttachmentService, TweetAttachment} from "../../services/http/attachment-service";

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
  pfpAttachment: TweetAttachment;

  private userService = inject(UserService)
  private activatedRoute = inject(ActivatedRoute)
  private tweetService = inject(TweetService)
  private attachmentService = inject(AttachmentService)

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.userId = Number(params.get("id"))

      this.loadUser()
      this.loadUserTweets()
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
      }, error: err => console.error(err)
    })
  }
}
