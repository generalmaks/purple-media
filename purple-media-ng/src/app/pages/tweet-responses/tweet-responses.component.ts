import {Component, inject, OnInit} from '@angular/core';
import {TweetComponent} from "../../components/tweet/tweet.component";
import {Tweet, TweetService} from "../../services/http/tweet.service";
import {ActivatedRoute} from '@angular/router'
import {CommonModule} from '@angular/common'
import {ComposerComponent} from "../../components/composer/composer.component";

@Component({
  selector: 'app-tweet-responses',
  standalone: true,
  imports: [
    TweetComponent,
    CommonModule,
    ComposerComponent,
  ],
  templateUrl: './tweet-responses.component.html',
  styleUrl: './tweet-responses.component.css'
})
export class TweetResponsesComponent implements OnInit {
  mainTweet: Tweet;
  responses: Tweet[] = [];

  private tweetService = inject(TweetService)
  private actRoute = inject(ActivatedRoute)

  ngOnInit() {
    this.loadTweets()
  }

  loadTweets() {
    let tweetId: number

    this.actRoute.paramMap.subscribe({
      next: params => {
        tweetId = Number(params.get('tweetId'))

        this.tweetService.get(tweetId).subscribe({
          next: tweet => {
            this.mainTweet = tweet!
          },
          error: err => console.error('Could not retrieve main tweet: ' + JSON.stringify(err))
        })

        this.tweetService.getResponses(tweetId).subscribe({
          next: responses => this.responses = responses,
          error: err => console.error('Could not get responses: ' + JSON.stringify(err))
        })
      },
      error: err => console.error('Could not retrieve tweet ID from url: ' + JSON.stringify(err))
    })
  }
}
