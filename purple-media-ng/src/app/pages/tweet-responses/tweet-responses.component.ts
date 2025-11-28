import { Component, Input } from '@angular/core';
import {TweetComponent} from "../../components/tweet/tweet.component";
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
    mainTweet!: string;
    responses: string[] = [];
  ngOnInit() {}
}
