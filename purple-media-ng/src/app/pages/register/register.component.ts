import {Component, inject, model} from '@angular/core';
import {AuthService, LoginDto, RegisterDto} from "../../services/http/auth.service";
import {Router} from '@angular/router';
import {FormsModule} from '@angular/forms';
import {AttachmentService} from "../../services/http/attachment-service";
import {NgIf} from "@angular/common";

export interface CreatePfpDto {
  userId: number,
  pfpFile: File
}

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, NgIf],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  selectedFile: File | null = null;
  pfpDto!: CreatePfpDto;

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

    this.auth.register(registerDto).subscribe(() => {
      const loginDto: LoginDto = {
        username: registerDto.username,
        unhashedPassword: registerDto.unhashedPassword
      }

      this.auth.login(loginDto).subscribe({
        next: () => {
          this.auth.me().subscribe({
            next: userDto => {
              if (this.selectedFile === null) {
                console.log("Registered")
                this.toLoginPage()
                return
              }
              this.attService.createPfp(userDto.id, this.selectedFile!).subscribe({
                error: err => console.error('Cant create pfp: ' + JSON.stringify(err))
              })
            }
          })
        }, error: err => console.error('Cant log in: ' + JSON.stringify(err))
      })
    }, err => console.error('Cant register account: ' + JSON.stringify(err)))
  }

  toLoginPage() {
    this.router.navigate(['/login'])
  }

  onFileSelected($event: Event) {
    if (event === null || event === undefined) return;

    const input = event.target as HTMLInputElement;

    if (!input.files || input.files.length === 0) {
      return;
    }

    this.selectedFile = input.files[0];


    console.log('Selected file:', this.selectedFile);
  }
}
