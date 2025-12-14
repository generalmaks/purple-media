import {Component, inject, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {AuthService} from "../../services/http/auth.service";
import {FormsModule} from '@angular/forms'
import {UserDto} from "../../services/http/user.service";
import {environment} from "../../../environment";

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent implements OnInit {
  searchQuery: string = ''
  user: UserDto | null = null
  pfpApiUrl = environment.apiUrl

  private auth = inject(AuthService)
  private router = inject(Router)

  ngOnInit() {
    this.loadUser()
  }

  isLoggedIn() {
    return this.auth.isLoggedIn()
  }

  loginLogout() {
    if (this.isLoggedIn())
      this.auth.logout()

    this.router.navigate(['/login'])
  }

  toMainPage() {
    this.router.navigate(['/'])
  }

  toChat() {
    this.router.navigate(['/chat']); // Adjust route as needed
  }

  toYoutube() {
    // Example external or internal link
    window.open('https://youtube.com', '_blank');
  }

  private loadUser() {
    if (!this.auth.isLoggedIn()) {
      this.user = null
      return
    }

    this.auth.me().subscribe({
      next: dto => this.user = dto,
      error: err => console.error('Could not get current users info: ' + JSON.stringify(err))
    })
  }
}
