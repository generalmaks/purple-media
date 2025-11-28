import { Component, OnInit } from '@angular/core';
import {TweetFeedComponent} from "../../components/tweet-feed/tweet-feed.component";
import {ComposerComponent} from "../../components/composer/composer.component";
import {TweetComponent} from "../../components/tweet/tweet.component";
import {NgIf, NgFor} from '@angular/common'
import {TweetService} from "../../services/tweet.service";
import {ActivatedRoute} from '@angular/router'

@Component({
  selector: 'app-search-feed',
  standalone: true,
  imports: [
    NgFor,
    NgIf,
    TweetFeedComponent,
  ],
  templateUrl: './search-feed.component.html',
  styleUrl: './search-feed.component.css'
})
export class SearchFeedComponent{
  tweets: string[] = []
  snippet: string | null = null

  ngOnInit(): void {
  }
}
