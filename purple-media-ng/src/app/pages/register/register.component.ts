import {Component, inject, model} from '@angular/core';
import {AuthService, RegisterDto} from "../../services/auth.service";
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
  private auth = inject(AuthService);
  private router = inject(Router)

  login = model<RegisterDto>({
    username: '',
    displayName: '',
    unhashedPassword: ''
  });

  submit() {
    const dto = this.login();

    this.auth.register(
      dto
    ).subscribe(() => {
      console.log("Registered")
      this.toLoginPage()
    })
  }

  toLoginPage() {
    this.router.navigate(['/login'])
  }
}
