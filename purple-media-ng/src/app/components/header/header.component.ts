import {Component, inject} from '@angular/core';
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

  private auth = inject(AuthService);

  isLoggedIn(){
    return this.auth.isLoggedIn();
  }
}
