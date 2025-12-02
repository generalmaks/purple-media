import { Component, OnInit } from '@angular/core';
import {NgIf, NgFor} from '@angular/common'
import {Tweet, TweetService} from "../../services/tweet.service";

@Component({
  selector: 'app-search-feed',
  standalone: true,
  imports: [
    NgFor,
    NgIf,
  ],
  templateUrl: './search-feed.component.html',
  styleUrl: './search-feed.component.css'
})
export class SearchFeedComponent{
  tweets: Tweet[] = []
  snippet: string | null = null

  ngOnInit(): void {
  }
}
