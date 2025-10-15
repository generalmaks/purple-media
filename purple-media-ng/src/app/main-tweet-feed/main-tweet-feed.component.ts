import { Component } from '@angular/core';
import {ComposerComponent} from "../composer/composer.component";
import {TweetFeedComponent} from "../tweet-feed/tweet-feed.component";
import {TweetService} from "../services/tweet.service";
import {TweetComponent} from "../tweet/tweet.component";
import {Tweet} from "../interfaces/tweet";

@Component({
  selector: 'app-main-tweet-feed',
  standalone: true,
  imports: [
    ComposerComponent,
    TweetFeedComponent,
  ],
  templateUrl: './main-tweet-feed.component.html',
  styleUrl: './main-tweet-feed.component.css'
})
export class MainTweetFeedComponent {
    tweets: Tweet[] = []

    constructor(private tweetService: TweetService) { }

  ngOnInit() {
      this.loadTweets()
  }

  loadTweets() {
    this.tweetService.getTweets().subscribe(
      (tweets) => {
        this.tweets = tweets
      },
      (error) => {
        console.error('Error fetching tweets:', error)
      }
    )
  }
}
