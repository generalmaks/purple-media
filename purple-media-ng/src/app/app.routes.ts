import {Routes} from '@angular/router';
import {UserPageComponent} from './pages/user-page/user-page.component';
import {LoginComponent} from "./pages/login/login.component";
import {RegisterComponent} from "./pages/register/register.component";
import {SearchFeedComponent} from "./pages/search-feed/search-feed.component";
import {MainTweetFeedComponent} from "./pages/main-tweet-feed/main-tweet-feed.component";
import {TweetResponsesComponent} from "./pages/tweet-responses/tweet-responses.component";

export const routes: Routes = [
  {path: '', component: MainTweetFeedComponent},
  {path: 'user/:id', component: UserPageComponent},
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'search/:snippet', component: SearchFeedComponent},
  {path: 'tweet/:tweetId', component: TweetResponsesComponent}
];
