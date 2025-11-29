import {Component, inject, Input} from '@angular/core';
import { CommonModule, NgIf } from '@angular/common';
import {Router} from "@angular/router";
import {Tweet} from "../../services/tweet.service";

@Component({
  selector: 'app-tweet',
  standalone: true,
  imports: [CommonModule, NgIf],
  templateUrl: './tweet.component.html',
  styleUrl: './tweet.component.css'
})
export class TweetComponent{
  @Input() tweet: Tweet = {
    id: 0,
    authorId: 0,
    content: "",
    createdAt: new Date()
  }

  private router = inject(Router)

  toUserProfile() {
    this.router.navigate([`/user/${this.tweet.authorId}`])
  }
}
