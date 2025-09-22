import { Component, Input } from '@angular/core';
import { LikesService } from '../services/likes.service';
import { DatePipe, NgIf } from '@angular/common';
import {AuthService} from "../services/auth.service";

@Component({
  selector: 'app-tweet',
  standalone: true,
  imports: [DatePipe, NgIf],
  templateUrl: './tweet.component.html',
  styleUrl: './tweet.component.css'
})
export class TweetComponent {
  @Input() tweet: any

  constructor(private likeService: LikesService, private authService: AuthService) {

  }
  handleLike() {
    if (!this.tweet || !this.tweet.postId) return;

    let currentUser = this.authService.getUsername()
    if (!currentUser) {
      console.error('You are not logged in!')
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
}
