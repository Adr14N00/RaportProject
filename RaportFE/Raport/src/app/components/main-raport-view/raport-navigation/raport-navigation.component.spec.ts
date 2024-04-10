import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RaportNavigationComponent } from './raport-navigation.component';

describe('RaportNavigationComponent', () => {
  let component: RaportNavigationComponent;
  let fixture: ComponentFixture<RaportNavigationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RaportNavigationComponent]
    });
    fixture = TestBed.createComponent(RaportNavigationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
