import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MainRaportViewComponent } from './main-raport-view.component';

describe('MainRaportViewComponent', () => {
  let component: MainRaportViewComponent;
  let fixture: ComponentFixture<MainRaportViewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MainRaportViewComponent]
    });
    fixture = TestBed.createComponent(MainRaportViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
