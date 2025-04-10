import { Component } from '@angular/core';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [NgFor],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {
  public buttons = [
    { link: "http://localhost:4200/user/general_maks", text: "😎Boss account" },
    { link: "http://youtube.com", text: "📺Youtube" }
  ]

  Clicked(button: { link: string; text: string }) {
    window.open(button.link, "_blank");
  }
}
