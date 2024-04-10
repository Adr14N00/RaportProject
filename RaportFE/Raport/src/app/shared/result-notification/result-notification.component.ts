import { animate, keyframes, sequence, style, transition, trigger } from '@angular/animations';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ResultNotification } from './models/result-notification.model';

@Component({
  selector: 'app-result-notification',
  templateUrl: './result-notification.component.html',
  styleUrls: ['./result-notification.component.scss'],
  animations: [
    trigger('fade', [
      transition( 'void => *', [
        style({ opacity: 1, right: 0  }),
        sequence([
          animate( 200,
            style({right: 10})
          ),
          animate( '{{time}}', keyframes([
            style({opacity: 0})
          ])),
        ])
      ]),
    ]),
  ]
})
export class ResultNotificationComponent {
  @Input() notificationProperties: ResultNotification = new ResultNotification();
  @Output() closeNotificationEvent = new EventEmitter<any>();

  constructor() {
  }

  ngOnInit(): void {
  }

  closeOverlay() {
    this.closeNotificationEvent.emit();
  }


}
