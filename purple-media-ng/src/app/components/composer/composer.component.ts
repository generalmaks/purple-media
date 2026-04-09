import {Component, EventEmitter, inject, Input, Output} from '@angular/core';
import {FormsModule} from '@angular/forms'
import {AttachmentService} from "../../services/http/attachment-service";
import {TweetService} from "../../services/http/tweet.service";
import {AuthService} from "../../services/http/auth.service";
import {NgForOf, NgIf} from "@angular/common";
import {catchError, forkJoin, of, switchMap} from "rxjs";

@Component({
  selector: 'app-composer',
  standalone: true,
  imports: [FormsModule, NgIf, NgForOf],
  templateUrl: './composer.component.html',
  styleUrl: './composer.component.css'
})
export class ComposerComponent {
  postContent: string = ''
  selectedFiles: File[] = []
  isSubmitting: boolean = false

  @Input() isInResponse: boolean = false
  @Input() responseTweetId: number | undefined = undefined;
  @Output() posted = new EventEmitter<void>()

  private tweetService = inject(TweetService)
  private attachmentService = inject(AttachmentService)
  private authService = inject(AuthService)

  onFilesSelected(event: Event) {
    const input = event.target as HTMLInputElement

    if (!input.files || input.files.length === 0) return;

    this.selectedFiles = Array.from(input.files)
  }

  submitPost() {
    if (!this.postContent.trim() || this.isSubmitting) return;

    this.isSubmitting = true;

    this.authService.me().pipe(
      switchMap(dto => this.tweetService.create(dto.id, this.postContent, this.responseTweetId)),
      switchMap(createdTweet => {
        if (this.selectedFiles.length === 0) {
          return of(createdTweet);
        }

        return forkJoin(
          this.selectedFiles.map(file =>
            this.attachmentService.create(createdTweet.id, file).pipe(
              catchError(err => {
                console.error('Attachment upload failed', err);
                return of(null);
              })
            )
          )
        );
      })
    ).subscribe({
      next: () => this.finishPosting(),
      error: err => {
        this.isSubmitting = false;
        console.error('Could not create post: ' + JSON.stringify(err));
      }
    });
  }

  resetForm() {
    this.postContent = ''
    this.selectedFiles = []
  }

  private finishPosting() {
    this.isSubmitting = false;
    this.resetForm();
    this.posted.emit()
  }
}
