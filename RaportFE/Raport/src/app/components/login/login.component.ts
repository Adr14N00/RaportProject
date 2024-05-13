import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { LoginModel } from 'src/app/models/login.model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {

  loginModel: LoginModel = new LoginModel();
  errorResponse?: string;

  constructor(private _authService: AuthService,
    private _router: Router) {}

  submit(loginModel: LoginModel) {
    this.errorResponse = undefined;
    this._authService.login(loginModel).subscribe({
      next: (data: any) => {
        if(data.Error){
          this.errorResponse = data.Error
        }
        else{
          this._router.navigate(['']);
        }
      },
      error: (err) => {
        this.errorResponse = err.error.Message
      }
    })
  }
}
