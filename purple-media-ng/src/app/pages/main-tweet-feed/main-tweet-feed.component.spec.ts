import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MainTweetFeedComponent } from './main-tweet-feed.component';

describe('MainTweetFeedComponent', () => {
  let component: MainTweetFeedComponent;
  let fixture: ComponentFixture<MainTweetFeedComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MainTweetFeedComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MainTweetFeedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
