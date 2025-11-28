import { Component } from '@angular/core';
import {ComposerComponent} from "../../components/composer/composer.component";
import {TweetFeedComponent} from "../../components/tweet-feed/tweet-feed.component";
import {TweetService} from "../../services/tweet.service";
import {TweetComponent} from "../../components/tweet/tweet.component";

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
  tweets : string[] = ['sex']
}
