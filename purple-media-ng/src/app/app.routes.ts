import { Routes } from '@angular/router';
import { TweetFeedComponent } from './tweet-feed/tweet-feed.component';
import { UserPageComponent } from './user-page/user-page.component';
import {LoginComponent} from "./pages/login/login.component";

export const routes: Routes = [
    { path: '', component: TweetFeedComponent },
    { path: 'user/:id', component: UserPageComponent },
    { path: 'login', component: LoginComponent}
];
