import { Component, Input, OnInit, OnChanges, SimpleChanges, ChangeDetectorRef } from '@angular/core';
import { LikesService } from '../../services/likes.service';
import { CommonModule, NgIf } from '@angular/common';
import { AuthService } from "../../services/auth.service";
import { ActivatedRoute, Router } from '@angular/router';
import { TweetService } from "../../services/tweet.service";
import { FileServiceService } from "../../services/file-service.service";

@Component({
  selector: 'app-tweet',
  standalone: true,
  imports: [CommonModule, NgIf],
  templateUrl: './tweet.component.html',
  styleUrl: './tweet.component.css'
})
export class TweetComponent implements OnInit, OnChanges {
  @Input() tweet: any;
  profilePictureUrl?: string;

  constructor(
    private likeService: LikesService,
    private authService: AuthService,
    private actRouter: ActivatedRoute,
    private router: Router,
    private tweetService: TweetService,
    private fileService: FileServiceService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    console.log('TweetComponent ngOnInit', this.tweet?.postId ?? '(no id)');
    this.tryLoadAll();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['tweet'] && changes['tweet'].currentValue) {
      console.log('TweetComponent ngOnChanges for postId', this.tweet?.postId);
      this.tryLoadAll();
    }
  }

  private tryLoadAll() {
    // run likes/responses only when tweet exists
    if (!this.tweet) return;
    this.getLikesAmount();
    this.getResponsesAmount();
    this.loadProfilePicture();
  }

  loadProfilePicture() {
    // IMPORTANT: check your real property name here!
    // Common variants: authorsProfilePictureID OR authorsProfilePictureId OR authorsProfilePictureUrl
    const id = this.tweet?.authorsProfilePictureID ?? this.tweet?.authorsProfilePictureId ?? this.tweet?.profilePictureId ?? null;

    console.log('loadProfilePicture() -> postId:', this.tweet?.postId, 'resolved pfp id:', id);

    if (!id) {
      console.warn('No profile picture id found for tweet', this.tweet?.postId);
      this.profilePictureUrl = undefined; // ensure fallback used
      return;
    }

    // debug log inside service too (see service suggestion below)
    this.fileService.getFile(id).subscribe({
      next: (blob: Blob) => {
        console.log('Received blob for post', this.tweet?.postId, 'type:', blob.type, 'size:', blob.size);
        // If backend returned JSON or empty, blob.type will show that
        if (!blob || blob.size === 0) {
          console.warn('Empty blob for pfp id', id);
          this.profilePictureUrl = undefined;
          this.cdr.detectChanges();
          return;
        }

        this.profilePictureUrl = URL.createObjectURL(blob);
        // force update just in case
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error loading profile picture for id', id, err);
      }
    });
  }

  handleLike() { /* keep your existing implementation */ }
  openResponses(): void { this.router.navigate(['/tweet/', this.tweet.postId]); }
  toUserPage() { if (this.tweet?.author) this.router.navigate(['/user/', this.tweet.author]); }

  getLikesAmount() {
    if (!this.tweet?.postId) return;
    this.likeService.getLikes(this.tweet.postId).subscribe({
      next: res => this.tweet.likedBy = res,
      error: err => console.error('Error fetching likes:', err)
    });
  }

  getResponsesAmount() {
    if (!this.tweet?.postId) return;
    this.tweetService.getResponsesToTweet(this.tweet.postId).subscribe({
      next: res => this.tweet.responses = res,
      error: err => console.error('Error fetching responses:', err)
    });
  }
}
