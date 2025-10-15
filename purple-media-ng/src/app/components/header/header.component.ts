import { Component } from '@angular/core';
import { Router } from '@angular/router';
import {AuthService} from "../../services/auth.service";
import { FormsModule } from '@angular/forms'

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  searchQuery: string = ''
  constructor(public authService: AuthService, private router: Router) { }

  onLoginLogout() {
    if(this.authService.isLoggedIn()){
      this.authService.logout();
      this.router.navigate(['/'])
    } else {
      this.router.navigate(['/login'])
    }
  }

  search() {
    if(this.searchQuery.trim().length > 0){
      this.router.navigate(['/search', this.searchQuery.trim()]);
    }
  }
  toMainPage(){
    this.searchQuery = ''
    this.router.navigate(['/'])
  }
}
