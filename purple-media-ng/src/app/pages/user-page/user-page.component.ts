import {Component, OnInit} from '@angular/core';
import {TweetFeedComponent} from "../../components/tweet-feed/tweet-feed.component";
import {ActivatedRoute} from '@angular/router';
import {UserService} from '../../services/user.service';
import {DatePipe, NgFor} from '@angular/common';
import {TweetService} from '../../services/tweet.service';
import {AttachmentService} from "../../services/attachment-service";

@Component({
  selector: 'app-user-page',
  standalone: true,
  imports: [DatePipe, TweetFeedComponent],
  templateUrl: './user-page.component.html',
  styleUrl: './user-page.component.css'
})
export class UserPageComponent implements OnInit {
  userId: string | null = ''
  user: any
  userTweets: any[] = []
  profilePictureUrl?: string
  
  ngOnInit(): void {
  }
}
