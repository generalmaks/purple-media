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

  constructor(private route: ActivatedRoute,
              private userService: UserService,
              private tweetService: TweetService,
              private fileService: AttachmentService) {
  }

  ngOnInit(): void {
    this.userId = this.route.snapshot.paramMap.get("id")
    if (this.userId) {
      this.loadUserData(this.userId)
    }
  }

  loadUserData(userId: string | null) {
    if (!userId) return;

    console.log("Getting tweets");
    this.tweetService.getTweetsByUser(userId).subscribe(
      res => {
        this.userTweets = res.map(tweet => ({
          ...tweet,
          authorsProfilePictureId: this.user?.profilePictureId
        }));
      },
      err => console.error(err)
    );

    console.log("Gettings user data")
    this.userService.getUserPublicInfo(userId).subscribe(res => {
      this.user = res;

      const pfpId = this.user?.profilePictureId;

      if (pfpId) {
        this.fileService.getFile(pfpId).subscribe({
          next: (blob: Blob) => {
            this.profilePictureUrl = URL.createObjectURL(blob);
          },
          error: err => console.error('Error loading profile picture:', err)
        });
      } else {
        console.warn('No profile picture ID found for user ', this.user);
      }
    });
  }
}
