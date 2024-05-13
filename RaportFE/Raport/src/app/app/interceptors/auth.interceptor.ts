import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { BehaviorSubject, Observable, catchError, filter, switchMap, take, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  private isRefreshing: boolean = false;
  private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(
    null
  );

  constructor(private _authService: AuthService) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (this._authService.token() == null) return next.handle(request);

    const authHeaderRequest = this.addAccessToken(request, this._authService.token()!!);
    
    return next.handle(authHeaderRequest).pipe(
      catchError((error) => {
        if (error instanceof (HttpErrorResponse) && error.status === 401) {
          if(error.error && error.error.ErrorCode == 1003) return throwError(error);
          return this.handle401Error(request, next);
        }
        else return throwError(error);
      })
    );
  }

  private handle401Error(request: HttpRequest<any>, next: HttpHandler) {
    if (this.isRefreshing) {
      return this.refreshTokenSubject.pipe(
        filter((token) => token != null),
        take(1),
        switchMap((jwt) => {
          const authHeaderRequest = this.addAccessToken(request, jwt);
          return next.handle(authHeaderRequest);
        })
      );
    }
    else {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);
      return this._authService.refreshToken().pipe(
        switchMap((response: any) => {
          this.isRefreshing = false;
          this.refreshTokenSubject.next(response.Token)
          const authHeaderRequest = this.addAccessToken(request, response.Token);
          return next.handle(authHeaderRequest);
        })
      )
    }
  }

  private addAccessToken(request: HttpRequest<any>, accessToken: string) {
    return request.clone({
      headers: request.headers.set('Authorization', "Bearer " + accessToken),
      withCredentials: true
    })
  }
}
