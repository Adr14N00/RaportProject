import { Injectable, signal } from '@angular/core';
import { Notification } from '../models/notification.model';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  infoSignal = signal<Notification | null>(null);
  successSignal = signal<Notification | null>(null);
  warningSignal = signal<Notification | null>(null);
  errorSignal = signal<Notification | null>(null);
  validationSignal = signal<Notification | null>(null);

}
