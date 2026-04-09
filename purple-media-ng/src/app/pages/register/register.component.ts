import {Component, inject, model} from '@angular/core';
import {AuthService, LoginDto, RegisterDto} from "../../services/http/auth.service";
import {Router} from '@angular/router';
import {FormsModule} from '@angular/forms';
import {AttachmentService} from "../../services/http/attachment-service";
import {NgIf} from "@angular/common";
import {of, switchMap} from "rxjs";

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, NgIf],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  selectedFile: File | null = null;

  login = model<RegisterDto>({
    username: '',
    displayName: '',
    unhashedPassword: ''
  });

  private auth = inject(AuthService);
  private router = inject(Router)
  private attService = inject(AttachmentService)

  submit() {
    const registerDto = this.login();
    const loginDto: LoginDto = {
      username: registerDto.username,
      unhashedPassword: registerDto.unhashedPassword
    };

    this.auth.register(registerDto).pipe(
      switchMap(() => this.auth.login(loginDto)),
      switchMap(() => this.selectedFile ? this.auth.me() : of(null)),
      switchMap(userDto => {
        if (!userDto || !this.selectedFile) {
          return of(null);
        }

        return this.attService.createPfp(userDto.id, this.selectedFile);
      })
    ).subscribe({
      next: () => this.toLoginPage(),
      error: err => console.error('Could not register account: ' + JSON.stringify(err))
    });
  }

  toLoginPage() {
    this.router.navigate(['/login'])
  }

  onFileSelected($event: Event) {
    const input = $event.target as HTMLInputElement | null;

    if (!input?.files || input.files.length === 0) {
      return;
    }

    this.selectedFile = input.files[0];
  }
}
