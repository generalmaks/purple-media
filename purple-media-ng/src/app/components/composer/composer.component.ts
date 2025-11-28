import {Component, Input} from '@angular/core';
import {AuthService} from "../../services/auth.service";
import {Router} from '@angular/router';
import {FormsModule} from '@angular/forms'
import {TweetService} from "../../services/tweet.service";

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
  @Input() isInResponse: boolean = false;
  @Input() responseTweetId: number = 0;
}
