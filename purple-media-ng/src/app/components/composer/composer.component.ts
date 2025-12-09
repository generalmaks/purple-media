import {Component, inject, Input} from '@angular/core';
import {FormsModule, NgForm} from '@angular/forms'
import {AttachmentService, TweetAttachment} from "../../services/http/attachment-service";
import {TweetService} from "../../services/http/tweet.service";
import {AuthService} from "../../services/http/auth.service";
import {NgForOf, NgIf} from "@angular/common";

@Component({
  selector: 'app-composer',
  standalone: true,
  imports: [FormsModule, NgIf, NgForOf],
  templateUrl: './composer.component.html',
  styleUrl: './composer.component.css'
})
export class ComposerComponent {
  postContent: string = ''
  selectedFiles: File[]

  @Input() isInResponse: boolean = false
  @Input() responseTweetId: number | undefined = undefined;

  private tweetService = inject(TweetService)
  private attachmentService = inject(AttachmentService)
  private authService = inject(AuthService)

  onFilesSelected(event: Event) {
    const input = event.target as HTMLInputElement

    if (!input.files || input.files.length === 0) return;

    this.selectedFiles = Array.from(input.files)
  }

  submitPost() {
    if (!this.postContent.trim()) return;

    let currentUserId: number
    this.authService.me().subscribe({
      next: dto => {
        currentUserId = dto.id

        this.tweetService.create(currentUserId, this.postContent, this.responseTweetId).subscribe({
          next: createdTweet => {
            if (this.selectedFiles.length === 0) {
              this.resetForm()
              return
            }

            let finished = 0

            for (const file of this.selectedFiles) {
              this.attachmentService.create(createdTweet.id, file).subscribe({
                next: () => {
                  finished++
                  if (finished === this.selectedFiles.length) {
                    this.resetForm()
                  }
                }, error: (err) => {
                  console.error("Attachment upload failed", err);
                  finished++;
                  if (finished === this.selectedFiles.length) {
                    this.resetForm();
                  }
                }
              })
            }
          }, error: err => console.error('Could not create tweet: ' + JSON.stringify(err))
        })
      },
      error: err => console.error('Could not determine current user: ' + JSON.stringify(err))
    })
  }

  resetForm() {
    this.postContent = ''
    this.selectedFiles = []
  }
}
