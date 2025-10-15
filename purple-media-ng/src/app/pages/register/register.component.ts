import { Component } from '@angular/core';
import {AuthService} from "../../services/auth.service";
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  username = '';
  password = '';
  email = '';

  constructor(private authService: AuthService, private router: Router) {}

  onRegister() {
    this.authService.register(this.username, this.password, this.email).subscribe({
      next: () => this.router.navigate(['/login']),
      error: err =>{
        console.error(err);
        alert(err.error.toString() || 'Registration failed')}
    });
  }

  toLogin() {
    this.router.navigate(['/login']);
  }
}
