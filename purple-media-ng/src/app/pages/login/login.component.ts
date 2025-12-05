import {Component, inject, model} from '@angular/core';
import { FormsModule } from '@angular/forms';
import {AuthService, LoginDto, RegisterDto} from "../../services/http/auth.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  standalone: true,
  imports: [FormsModule]
})
export class LoginComponent {
  private auth = inject(AuthService);
  private router = inject(Router)

  login = model<LoginDto>({
    username: '',
    unhashedPassword: ''
  });

  submit() {
    const dto = this.login();

    this.auth.login(dto).subscribe(() => {
      console.log("Logged in")
      this.router.navigate(['/'])
    })
  }

  toRegisterPage() {
    this.router.navigate(['/register'])
  }
}
