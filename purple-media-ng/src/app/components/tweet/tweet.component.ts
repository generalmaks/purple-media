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
  @Input() tweet: Tweet

  private router = inject(Router)

  toUserProfile() {
    this.router.navigate([`/user/${this.tweet.authorId}`])
  }
}
