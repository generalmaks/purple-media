import {Component, inject, OnInit} from '@angular/core';
import {ComposerComponent} from "../../components/composer/composer.component";
import {TweetService} from "../../services/tweet.service";
import {TweetComponent} from "../../components/tweet/tweet.component";

@Component({
  selector: 'app-main-tweet-feed',
  standalone: true,
  imports: [
    ComposerComponent,
    TweetComponent,
  ],
  templateUrl: './main-tweet-feed.component.html',
  styleUrl: './main-tweet-feed.component.css'
})
export class MainTweetFeedComponent implements OnInit{
  tweets : any[] = []
  private readonly pageSize = 10
  private page = 0
  private loading = false

  private tweetService = inject(TweetService)

  ngOnInit() {
    this.loadMore()
  }

  loadMore(){
    if(this.loading) return;
    this.loading = true

    this.tweetService.getLatest(this.page, this.pageSize).subscribe(tweets => {
      this.tweets.push(...tweets)
      this.page++
      this.loading = false
    })
  }

  onScroll(event: Event) {
    const el = event.target as HTMLElement;

    const atBottom =
      el.scrollTop + el.clientHeight >= el.scrollHeight - 50;

    if (atBottom && !this.loading) {
      this.loadMore();
    }
  }

}
