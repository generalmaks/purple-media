import { Component } from '@angular/core';
import { Router } from '@angular/router';
import {AuthService} from "../services/auth.service";

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  constructor(public authService: AuthService, private router: Router) { }

  onLoginLogout() {
    if(this.authService.isLoggedIn()){
      this.authService.logout();
      this.router.navigate(['/'])
    } else {
      this.router.navigate(['/login'])
    }
  }
}
