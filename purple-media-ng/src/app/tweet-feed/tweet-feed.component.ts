import { Component, Input, OnInit } from '@angular/core';
import { CommonModule, NgFor, NgIf } from '@angular/common';
import { TweetService } from '../services/tweet.service';
import { TweetComponent } from "../tweet/tweet.component";

@Component({
  selector: 'app-tweet-feed',
  standalone: true,
  imports: [NgIf, NgFor, TweetComponent],
  templateUrl: './tweet-feed.component.html',
  styleUrl: './tweet-feed.component.css'
})
export class TweetFeedComponent implements OnInit {
  @Input() userId: string | null = null
  tweets: any[] = []
  constructor(private tweetService: TweetService) { }

  ngOnInit(): void {
    this.loadTweets()
  }

  loadTweets() {
    if (this.userId) {
      this.tweetService.getTweetsByUser(this.userId).subscribe(
        (tweets) => {
          this.tweets = tweets
        },
        (error) => {
          console.error(error)
        }
      )
    } else {
      this.tweetService.getTweets().subscribe(
        (data) => {
          this.tweets = data
        },
        (error) => {
          console.error('Error fetching tweets:', error)
        }
      )
    }
  }
}
