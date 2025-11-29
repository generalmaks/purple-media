import {Component, inject, Input, OnInit} from '@angular/core';
import {CommonModule, NgIf, NgOptimizedImage} from '@angular/common';
import {Router} from "@angular/router";
import {Tweet} from "../../services/tweet.service";
import {AttachmentService, TweetAttachment} from "../../services/attachment-service";
import {DomSanitizer} from "@angular/platform-browser";
import {environment} from "../../../environment";

@Component({
  selector: 'app-tweet',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './tweet.component.html',
  styleUrl: './tweet.component.css'
})
export class TweetComponent implements OnInit{
  @Input() tweet: Tweet
  attachments: TweetAttachment[] = []
  apiUrl = environment.apiUrl


  private router = inject(Router)
  private attachService = inject(AttachmentService)

  ngOnInit() {
    this.attachService.getForTweet(this.tweet.id).subscribe({
      next: (atts) => {
        this.attachments = atts
      }, error : err => console.error("Failed at retreiving tweet attachments: " + err)
    })
  }

  toUserProfile() {
    this.router.navigate([`/user/${this.tweet.authorId}`])
  }

  isImage(att: TweetAttachment): boolean {
    return att.mediaType.startsWith("image/")
  }
}
