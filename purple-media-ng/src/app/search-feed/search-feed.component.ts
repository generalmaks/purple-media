import { Component, OnInit } from '@angular/core';
import {TweetFeedComponent} from "../tweet-feed/tweet-feed.component";
import {ComposerComponent} from "../composer/composer.component";
import {TweetComponent} from "../tweet/tweet.component";
import {NgIf, NgFor} from '@angular/common'
import {TweetService} from "../services/tweet.service";
import {ActivatedRoute} from '@angular/router'
import {Tweet} from "../interfaces/tweet";

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
  tweets: Tweet[] = []
  snippet: string | null = null

  constructor(
    private tweetService: TweetService,
    private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.snippet = params.get("snippet");
      if(this.snippet){
        this.loadSearchedTweets(this.snippet);
      }
    })
  }

  loadSearchedTweets(query: string) {
    this.tweetService.searchTweets(query).subscribe(results => {
      let tweetsInfo = results.map(r => r.post)
      this.tweets = tweetsInfo;
    }, error => {
      console.error(error);
      alert('Snippet search failed. Try again.');
    })
  }
}
