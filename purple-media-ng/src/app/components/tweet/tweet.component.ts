import { Component, Input, OnChanges } from '@angular/core';
import { LikesService } from '../../services/likes.service';
import { CommonModule, NgIf } from '@angular/common';
import {AuthService} from "../../services/auth.service";
import { ActivatedRoute, Router } from '@angular/router'
import {TweetService} from "../../services/tweet.service";
import { AttachmentService} from "../../services/attachment-service";

@Component({
  selector: 'app-tweet',
  standalone: true,
  imports: [CommonModule, NgIf],
  templateUrl: './tweet.component.html',
  styleUrl: './tweet.component.css'
})
export class TweetComponent{
  @Input() tweet: any
  profilePictureUrl?: string;
}
