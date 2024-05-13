import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component } from '@angular/core';
import { AbstractControl, FormGroupDirective, NgForm } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { ChangePasswordModel } from 'src/app/models/change-password.model';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss'],
  animations: [
    trigger('openClose', [
      state('false', style({
        transform: 'translatex(0px)'
      })),
      state('true', style({
        transform: 'translatex(-400px)'
      })),
      transition('false => true', [
        animate('0.2s ease-in-out')
      ])
    ])
  ]
})
export class ChangePasswordComponent {
  changePasswordModel: ChangePasswordModel = new ChangePasswordModel();
  samePasswordStateMatcher: SamePasswordErrorStateMatcher = new SamePasswordErrorStateMatcher();
  errorResponse: string = "";

  isDone = false;

  constructor(private _userService: UserService) { }

  toggle(){
    this.isDone = !this.isDone;
  }

  submit(model: ChangePasswordModel) {
    this._userService.changeUserPassword(model).subscribe({
      next: () => this.toggle(),
      error: (error) => {
        console.log(error)
        this.errorResponse = error.error.Message;
      }
    })
  }
}

export class SamePasswordErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: AbstractControl<any, any> | null, form: FormGroupDirective | NgForm | null): boolean {
    return !!(form?.hasError('differentPassword') && control?.dirty);
  }
}
