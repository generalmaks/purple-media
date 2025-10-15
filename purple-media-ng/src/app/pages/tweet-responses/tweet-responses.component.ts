import { Component, Input } from '@angular/core';
import {TweetComponent} from "../../components/tweet/tweet.component";
import {Tweet} from "../../interfaces/tweet";
import {TweetService} from "../../services/tweet.service";
import {ActivatedRoute} from '@angular/router'
import { CommonModule } from '@angular/common'

@Component({
  selector: 'app-tweet-responses',
  standalone: true,
  imports: [
    TweetComponent,
    CommonModule,
  ],
  templateUrl: './tweet-responses.component.html',
  styleUrl: './tweet-responses.component.css'
})
export class TweetResponsesComponent {
    @Input() mainTweet!: Tweet;
    @Input() responses: Tweet[] = [];

    constructor(
      private tweetService: TweetService,
      private actRoute: ActivatedRoute,) {}

    ngOnInit() {
      this.loadMainTweet()
      this.loadResponseTweets()
    }

    loadMainTweet() {
      let thisTweetIdUrl = this.actRoute.snapshot.paramMap.get("tweetId")
      if (!thisTweetIdUrl) {
        alert('Tweet not found')
        return
      }
      let thisTweetId = thisTweetIdUrl;

      this.tweetService.getTweetsById(thisTweetId).subscribe({
        next: res => {
          this.mainTweet = res;
        },
        error: (err) => {
          console.error('Error loading tweet:', err);
          alert('Failed to load tweet');
        }
      });
    }

  loadResponseTweets() {
    let thisTweetIdUrl = this.actRoute.snapshot.paramMap.get("tweetId")
    if (!thisTweetIdUrl) {
      alert('Tweet not found')
      return
    }
    let thisTweetId = thisTweetIdUrl;

    this.tweetService.getResponsesToTweet(thisTweetId).subscribe({
      next: res => {
        this.responses = res;
      }, error: (err) => {
        console.error('Error loading tweet:', err);
        alert('Failed to load tweet');
      }
    })
  }
}
