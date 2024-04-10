import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExportsListComponent } from './exports-list.component';

describe('ExportsListComponent', () => {
  let component: ExportsListComponent;
  let fixture: ComponentFixture<ExportsListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ExportsListComponent]
    });
    fixture = TestBed.createComponent(ExportsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
