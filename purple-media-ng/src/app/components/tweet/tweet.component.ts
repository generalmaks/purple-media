import { Component, Input, OnChanges } from '@angular/core';
import { LikesService } from '../../services/likes.service';
import { CommonModule, NgIf } from '@angular/common';
import {AuthService} from "../../services/auth.service";
import { ActivatedRoute, Router } from '@angular/router'
import {TweetService} from "../../services/tweet.service";

@Component({
  selector: 'app-tweet',
  standalone: true,
  imports: [CommonModule, NgIf],
  templateUrl: './tweet.component.html',
  styleUrl: './tweet.component.css'
})
export class TweetComponent implements OnChanges{
  @Input() tweet: any

  constructor(
    private likeService: LikesService,
    private authService: AuthService,
    private actRouter: ActivatedRoute,
    private router: Router,
    private tweetService: TweetService) {
  }

  ngOnChanges() {
    this.getLikesAmount()
    this.getResponsesAmount()
  }
  handleLike() {
    if (!this.tweet || !this.tweet.postId) return;

    let currentUser = this.authService.getUsername()
    if (!currentUser) {
      console.error('You are not logged in!')
      this.router.navigate(['/login'])
      return;
    }
    currentUser = String(currentUser)

    this.likeService.likePost(this.tweet.postId, currentUser).subscribe({
      next: () => {
        this.likeService.getLikes(this.tweet.postId).subscribe({
          next: (likes: string[]) => {
            this.tweet.likedBy = [...likes];
          },
          error: (err) => console.error('Error fetching likes:', err)
        });
      },
      error: (err) => console.error('Error liking tweet:', err)
    });
  }

  openResponses(): void{
    this.router.navigate(['/tweet/', this.tweet.postId]);
  }

  toUserPage() {
    if (this.tweet.author)
    this.router.navigate(['/user/', this.tweet.author]);
  }

  getLikesAmount() {
    this.likeService.getLikes(this.tweet.postId).subscribe({
      next: res => {
        this.tweet.likedBy = res;
      }, error: (err) => {
        console.error('Error fetching likes:', err)
      }
    })
  }

  getResponsesAmount() {
    this.tweetService.getResponsesToTweet(this.tweet.postId).subscribe({
      next: res => {
        this.tweet.responses = res;
      }
    })
  }
}
