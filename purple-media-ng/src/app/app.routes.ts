import {Routes} from '@angular/router';
import {TweetFeedComponent} from './tweet-feed/tweet-feed.component';
import {UserPageComponent} from './user-page/user-page.component';
import {LoginComponent} from "./pages/login/login.component";
import {RegisterComponent} from "./pages/register/register.component";
import {SearchFeedComponent} from "./search-feed/search-feed.component";
import {MainTweetFeedComponent} from "./main-tweet-feed/main-tweet-feed.component";

export const routes: Routes = [
  {path: '', component: MainTweetFeedComponent},
  {path: 'user/:id', component: UserPageComponent},
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'search/:snippet', component: SearchFeedComponent}
];
