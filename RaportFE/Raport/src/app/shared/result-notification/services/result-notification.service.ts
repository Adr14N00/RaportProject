import { Injectable, ViewContainerRef, signal } from '@angular/core';
import { ResultNotificationComponent } from '../result-notification.component';
import { ResultNotification } from '../models/result-notification.model';
import { MatDialog } from '@angular/material/dialog';

@Injectable({
  providedIn: 'root'
})
export class ResultNotificationService {
  constructor(
    private dialog: MatDialog
  ) {

  }

  openDialog(properties: Notification): void {
      const dialogRef = this.dialog.open(ResultNotificationComponent, {
        data: {properties},
        maxHeight: '100%',
        width: '540px',
        maxWidth: '100%',
        disableClose: true,
        hasBackdrop: true,
      });
  }

  initNotification(properties: ResultNotification, viewContainerRef: ViewContainerRef) {
    let notificationRef = viewContainerRef.createComponent<ResultNotificationComponent>(ResultNotificationComponent);
    notificationRef.instance.notificationProperties = properties;
    notificationRef.instance.closeNotificationEvent.subscribe(() => {
      notificationRef.destroy()
    })
  }

}
