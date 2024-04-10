/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { ResultNotificationService } from './result-notification.service';

describe('Service: ResultNotification', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ResultNotificationService]
    });
  });

  it('should ...', inject([ResultNotificationService], (service: ResultNotificationService) => {
    expect(service).toBeTruthy();
  }));
});
