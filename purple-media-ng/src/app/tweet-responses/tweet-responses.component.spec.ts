import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TweetResponsesComponent } from './tweet-responses.component';

describe('TweetResponsesComponent', () => {
  let component: TweetResponsesComponent;
  let fixture: ComponentFixture<TweetResponsesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TweetResponsesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TweetResponsesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
