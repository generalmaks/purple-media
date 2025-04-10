import { Component, Input } from '@angular/core';
import { LikesService } from '../services/likes.service';
import { DatePipe, NgIf } from '@angular/common';

@Component({
  selector: 'app-tweet',
  standalone: true,
  imports: [DatePipe, NgIf],
  templateUrl: './tweet.component.html',
  styleUrl: './tweet.component.css'
})
export class TweetComponent {
  @Input() tweet: any

  constructor(private likeService: LikesService) {

  }
  handleLike() {
    if (this.tweet && this.tweet.postId) {
      this.likeService.likePost(this.tweet.postId, "ZvBnVhY").subscribe({
        next: () => {
          this.tweet.likedBy.push("ZvBnVhY")
        },
        error: (err) => console.error('Error liking tweet:', err)
      })
    }
  }
}
