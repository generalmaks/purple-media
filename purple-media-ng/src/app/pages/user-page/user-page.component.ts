import {Component, inject, OnInit} from '@angular/core';
import {User, UserService} from '../../services/user.service';
import {DatePipe} from '@angular/common';
import {ActivatedRoute} from "@angular/router";
import {Tweet, TweetService} from "../../services/tweet.service";
import {TweetComponent} from "../../components/tweet/tweet.component";

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
  profilePictureUrl?: string

  private userService = inject(UserService)
  private activatedRoute = inject(ActivatedRoute)
  private tweetService = inject(TweetService)

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
      this.profilePictureUrl = user?.profilePictureUrl
    })
  }

  private loadUserTweets() {
    this.tweetService.getUserTweets(this.userId).subscribe(tweets => {
      this.userTweets = tweets
    })
  }
}
