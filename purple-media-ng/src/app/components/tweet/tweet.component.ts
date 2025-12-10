import {Component, inject, Input, OnChanges, OnInit, SimpleChanges} from '@angular/core';
import {CommonModule} from '@angular/common';
import {Router} from "@angular/router";
import {Tweet} from "../../services/http/tweet.service";
import {AttachmentService, TweetAttachment} from "../../services/http/attachment-service";
import {environment} from "../../../environment";
import {User, UserService} from "../../services/http/user.service";
import {AuthService} from "../../services/http/auth.service";
import {FollowService} from "../../services/http/follow.service";
import {LikeService} from "../../services/http/like.service";
import {combineLatest, switchMap, tap} from "rxjs";

@Component({
  selector: 'app-tweet',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './tweet.component.html',
  styleUrl: './tweet.component.css'
})
export class TweetComponent implements OnChanges {
  @Input() tweet: Tweet
  user: User
  attachments: TweetAttachment[] = []
  pfpAttachment: TweetAttachment | null
  likesAmount: number
  apiUrl = environment.apiUrl
  isCurrentUserFollowingAuthorBool: boolean = false
  isCurrentUserLiked: boolean = false


  private router = inject(Router)
  private attachService = inject(AttachmentService)
  private userService = inject(UserService)
  private authService = inject(AuthService)
  private followService = inject(FollowService)
  private likeService = inject(LikeService)

  ngOnChanges(changes: SimpleChanges) {
    if (!this.tweet) return;

    this.loadTweetData();
  }

  likePost() {
    this.authService.me().pipe(
      switchMap(me =>
        this.likeService.isLiked(me.id, this.tweet.id).pipe(
          switchMap(isLiked => {
            if (isLiked) {
              return this.likeService.unlike(me.id, this.tweet.id).pipe(
                tap(() => {
                  this.likesAmount--;
                  this.isCurrentUserLiked = false;
                })
              );
            } else {
              return this.likeService.like(me.id, this.tweet.id).pipe(
                tap(() => {
                  this.likesAmount++;
                  this.isCurrentUserLiked = true;
                })
              );
            }
          })
        )
      )
    ).subscribe();
  }


  isImage(att: TweetAttachment): boolean {
    return att.mediaType.startsWith("image/");
  }

  toUserProfile() {
    this.router.navigate([`/user/${this.tweet.authorId}`]);
  }

  private loadTweetData() {
    combineLatest([
      this.userService.get(this.tweet.authorId),
      this.attachService.getForTweet(this.tweet.id),
      this.likeService.countLikes(this.tweet.id),
      this.authService.me(),
    ]).subscribe(([user, attachments, likes, me]) => {
      this.user = user!;
      this.attachments = attachments;
      this.likesAmount = likes;

      this.loadPfp();

      this.likeService.isLiked(me.id, this.tweet.id)
        .subscribe(isLiked => this.isCurrentUserLiked = isLiked);

      this.followService.isFollowing(me.id, user!.id)
        .subscribe(isFollowing => this.isCurrentUserFollowingAuthorBool = isFollowing);
    });
  }

  private loadPfp() {
    this.attachService.getForPfp(this.tweet.authorId)
      .subscribe({
        next: att => this.pfpAttachment = att,
        error: err => this.pfpAttachment = null
      });
  }
}
