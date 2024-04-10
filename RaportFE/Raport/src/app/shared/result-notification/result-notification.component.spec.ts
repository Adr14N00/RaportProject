import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ResultNotificationComponent } from './result-notification.component';

describe('ResultNotificationComponent', () => {
  let component: ResultNotificationComponent;
  let fixture: ComponentFixture<ResultNotificationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ResultNotificationComponent]
    });
    fixture = TestBed.createComponent(ResultNotificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
