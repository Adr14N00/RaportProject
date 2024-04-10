import { TestBed } from '@angular/core/testing';

import { ExportsServiceService } from './exports-service.service';

describe('ExportsServiceService', () => {
  let service: ExportsServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ExportsServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
