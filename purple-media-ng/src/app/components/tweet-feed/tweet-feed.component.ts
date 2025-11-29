import { Component, Input, OnChanges } from '@angular/core';
import { CommonModule, NgFor, NgIf } from '@angular/common';
import {Tweet, TweetService} from '../../services/tweet.service';
import { TweetComponent } from "../tweet/tweet.component";
import { ComposerComponent } from "../composer/composer.component";

@Component({
  selector: 'app-tweet-feed',
  standalone: true,
  imports: [NgIf, NgFor, TweetComponent],
  templateUrl: './tweet-feed.component.html',
  styleUrl: './tweet-feed.component.css'
})
export class TweetFeedComponent {
  @Input() userId: string | null = null
  @Input() tweets: Tweet[] = []
  constructor(private tweetService: TweetService) { }

}
