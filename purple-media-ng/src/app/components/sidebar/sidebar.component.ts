import { Component } from '@angular/core';
import { NgFor } from '@angular/common';

export interface SidebarButton {
  link: string,
  label: string,
  openOnNewPage: boolean
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [NgFor],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {
  public buttons: SidebarButton[] = [
    { link: "http://localhost:4200/user/general_maks", label: "😎Boss account", openOnNewPage: true },
    { link: "http://youtube.com", label: "📺Youtube", openOnNewPage: true },
    { link: "/chat", label: "💬Chat", openOnNewPage: false}
  ]

  Clicked(button: SidebarButton) {
    window.open(button.link, button.openOnNewPage ? "_blank" : "_self");
  }
}
