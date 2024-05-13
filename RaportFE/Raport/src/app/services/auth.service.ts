import { Injectable, effect, signal } from '@angular/core';
import { LoginModel } from '../models/login.model';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { EMPTY, catchError, tap } from 'rxjs';
import { Router } from '@angular/router';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = environment.SbManagementApiUrl + '/auth';

  token = signal<string | null>(null);
  userEmail = signal<string | null>(null);
  isLoggedIn = signal<boolean>(false);
  isPasswordSet?: boolean;
  role?: string;


  constructor(private _http: HttpClient, private _router: Router) {
    this.getLocalStorageData();
    effect(() =>{
      if(this.token()){
        localStorage.setItem("accessToken", this.token()!!)
      }
    })
  }

  login(loginModel: LoginModel) {
    return this._http.post(this.baseUrl + '/login', loginModel).pipe(
      tap((val: any) => {
        this.token.set(val.Token)
        this.userEmail.set(val.Email);
        this.role = val.Role;
        this.isLoggedIn.set(val.IsLoggedIn);
        this.isPasswordSet = val.isPasswordSet;
        this.setLocalStorageData(val);
      }))
  }

  logout() {
    this.isLoggedIn.set(false);
    localStorage.removeItem('accessToken');
    localStorage.removeItem('isLoggedIn');
    localStorage.removeItem('role');
    localStorage.removeItem('email');
    return this._http.put(this.baseUrl + '/revoke-token', {});
  }

  refreshToken() {
    return this._http.get(this.baseUrl + '/refresh-token', { withCredentials: true }).pipe(
      tap((val: any) => {
        this.token.set(val.Token)
        this.role = val.Role;
        this.userEmail = val.Email;
      }),
      catchError((err, caught) => {
        this.logout()
        this._router.navigate(['login'])
        return EMPTY;
      })
    );
  }

  private getLocalStorageData() {
    this.token.set(localStorage.getItem('accessToken') || null);
    this.isLoggedIn?.set(localStorage.getItem('isLoggedIn') === 'true');
    this.isPasswordSet = localStorage.getItem('isPasswordSet') === 'true' || undefined;
    this.role = localStorage.getItem('role') || undefined;
    this.userEmail.set(localStorage.getItem('email'));
  }

  private setLocalStorageData(data: any) {
    localStorage.setItem('accessToken', data.Token);
    localStorage.setItem('isLoggedIn', data.IsLoggedIn);
    localStorage.setItem('role', data.Role);
    localStorage.setItem('email', data.Email);
  }
}
