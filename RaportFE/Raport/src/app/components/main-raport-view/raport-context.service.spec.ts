/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { RaportContextService } from './raport-context.service';

describe('Service: RaportContext', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RaportContextService]
    });
  });

  it('should ...', inject([RaportContextService], (service: RaportContextService) => {
    expect(service).toBeTruthy();
  }));
});
