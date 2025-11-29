import {Component, inject, OnInit} from '@angular/core';
import {User, UserService} from '../../services/user.service';
import {DatePipe} from '@angular/common';

@Component({
  selector: 'app-user-page',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './user-page.component.html',
  styleUrl: './user-page.component.css'
})
export class UserPageComponent implements OnInit {
  userId: number | null = 0
  user: User
  userTweets: any[] = []
  profilePictureUrl?: string

  private userService = inject(UserService)

  ngOnInit(): void {
    this.userService.get(this.userId!).subscribe(user => {
      this.user = user!;
    })
  }
}
