import { NotificationService } from './services/notification.service';
import { Component, effect, signal } from '@angular/core';
import { Notification } from './models/notification.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrl: './notification.component.scss',
})
export class NotificationComponent {

  info: Notification[] = new Array();
  success: Notification[] = new Array();
  warning: Notification[] = new Array();
  error?: Notification[] = new Array();
  validation: Notification[] = new Array();

  isLoggedIn?: boolean;

  constructor(
    private notificationService: NotificationService,
    private router: Router,
  ){
    effect(() => {
      if(this.notificationService.infoSignal() != null){
        let signal = this.notificationService.infoSignal();
        this.info?.push(new Notification({message: signal?.message, type: signal?.type}));
      };
      if(this.notificationService.successSignal() != null){
        let signal = this.notificationService.successSignal();
        this.success?.push(new Notification({message: signal?.message, type: signal?.type}));
      };
      if(this.notificationService.warningSignal() != null){
        let signal = this.notificationService.warningSignal();
        this.warning?.push(new Notification({message: signal?.message, type: signal?.type}));
      };
      if(this.notificationService.errorSignal() != null){
        let signal = this.notificationService.errorSignal();
        this.error?.push(new Notification({message: signal?.message, type: signal?.type}));
      };
      if(this.notificationService.validationSignal() != null){
        let signal = this.notificationService.validationSignal();
        this.validation?.push(new Notification({message: signal?.message, type: signal?.type}));
      };

    })

    this.router.events.subscribe(() => {
      this.success = new Array();
      this.warning = new Array();
      this.error = new Array();
      this.validation = new Array();
    });
  }

  deleteInfo(index: number){
    this.info?.splice(index, 1);
  }

  deleteSuccess(index: number){
    this.success?.splice(index, 1);
  }

  deleteWarning(index: number){
    this.warning?.splice(index, 1);
  }

  deleteError(index: number){
    this.error?.splice(index, 1);
  }

  deleteValidation(index: number){
    this.validation?.splice(index, 1);
  }

  clearNotifications(){
    this.info = new Array();
    this.success = new Array();
    this.warning = new Array();
    this.error = new Array();
    this.validation = new Array();
  }

}
