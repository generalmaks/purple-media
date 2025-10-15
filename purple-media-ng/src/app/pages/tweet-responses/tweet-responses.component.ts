import { Component, Input } from '@angular/core';
import {TweetComponent} from "../../components/tweet/tweet.component";
import {Tweet} from "../../interfaces/tweet";
import {TweetService} from "../../services/tweet.service";
import {ActivatedRoute} from '@angular/router'
import { CommonModule } from '@angular/common'
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
export class TweetResponsesComponent {
    mainTweet!: Tweet;
    responses: Tweet[] = [];

    constructor(
      private tweetService: TweetService,
      private actRoute: ActivatedRoute,) {}

  ngOnInit() {
    this.actRoute.paramMap.subscribe(params => {
      const tweetId = params.get('tweetId');
      if (tweetId) {
        this.loadMainTweet(tweetId);
        this.loadResponseTweets(tweetId);
      }
    });
  }

    loadMainTweet(tweetIdUrl: string) {
      this.tweetService.getTweetsById(tweetIdUrl).subscribe({
        next: res => {
          this.mainTweet = res;
        },
        error: (err) => {
          console.error('Error loading tweet:', err);
          alert('Failed to load tweet');
        }
      });
    }

  loadResponseTweets(tweetIdUrl: string) {
    this.tweetService.getResponsesToTweet(tweetIdUrl).subscribe({
      next: res => {
        this.responses = this.sortByDateAndEngagement(res)
      }, error: (err) => {
        console.error('Error loading tweet:', err);
        alert('Failed to load tweet');
      }
    })
  }

  sortByDateAndEngagement(tweets: Tweet[]): Tweet[] {
    return tweets.sort((a: Tweet, b: Tweet) => {
      const engagementDiff = (b.likedBy.length + b.responses.length) - (a.likedBy.length + a.responses.length);
      if (engagementDiff !== 0) return engagementDiff;

      return new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime();
    });
  }

}
