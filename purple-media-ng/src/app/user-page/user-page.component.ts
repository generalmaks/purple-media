import { Component, OnInit } from '@angular/core';
import { TweetFeedComponent } from "../tweet-feed/tweet-feed.component";
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../services/user.service';
import { DatePipe, NgFor } from '@angular/common';
import { TweetService } from '../services/tweet.service';

@Component({
  selector: 'app-user-page',
  standalone: true,
  imports: [DatePipe, TweetFeedComponent],
  templateUrl: './user-page.component.html',
  styleUrl: './user-page.component.css'
})
export class UserPageComponent implements OnInit {
  userId: string | null = ''
  user: any
  userTweets: any[] = []

  constructor(private route: ActivatedRoute, private userService: UserService, private tweetService: TweetService) { }

  ngOnInit(): void {
    this.userId = this.route.snapshot.paramMap.get("id")
    if (this.userId) {
      this.loadUserData(this.userId)

    }
  }

  loadUserData(userId: string | null) {
    if (userId) {
      this.userService.getTweets(userId).subscribe(
        (data) => {
          this.user = data
        },
        (error) => {
          console.error(error)
        }
      )
    }
  }
}
