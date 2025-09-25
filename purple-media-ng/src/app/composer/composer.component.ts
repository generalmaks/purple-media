import { Component } from '@angular/core';
import {AuthService} from "../services/auth.service";
import { Router } from '@angular/router';
import {PostService} from "../services/post.service";
import { FormsModule } from '@angular/forms'

const Visibility: string[] = [
  'PUBLIC',
  'FRIENDS',
  'PRIVATE',
]

@Component({
  selector: 'app-composer',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './composer.component.html',
  styleUrl: './composer.component.css'
})
export class ComposerComponent {
  visibility: string = 'PUBLIC';
  postContent: string = ''

  constructor(private authService: AuthService, private router: Router, private postService: PostService) {
  }

  onChangeVisibility(): void {
    const nextIndex: number = (Visibility.findIndex(v => v === this.visibility) + 1) % Visibility.length;
    this.visibility = Visibility[nextIndex];
  }

  onPost(): void {
    if(!this.authService.isLoggedIn()) {
      alert("You are not logged in.");
      this.router.navigate(['/login']);
    } else {
      let postData = {
        authorId: this.authService.getUsername(),
        content: this.postContent,
        parentPostId: null
      }
      this.postService.postTweet(postData).subscribe({
        next: res => {
          console.log("Successfully posted")
          this.postContent = '';
          window.location.reload();
        },
        error: err => {
          console.error(err);
          alert("Failed to post");
        }
      })
    }
  }
}
