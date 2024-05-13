import { Directive, forwardRef } from '@angular/core';
import { AbstractControl, AsyncValidator, NG_ASYNC_VALIDATORS, ValidationErrors, Validators } from '@angular/forms';
import { Observable, catchError, map, of, tap } from 'rxjs';
import { UserService } from '../services/user.service';

@Directive({
  selector: '[appEmailAvailability]',
  providers: [
    {
      provide: NG_ASYNC_VALIDATORS,
      useExisting: forwardRef(() => EmailAvailabilityDirective),
      multi: true
    }
  ]
})
export class EmailAvailabilityDirective implements AsyncValidator {

  constructor(private userService: UserService) { }

  validate(control: AbstractControl): Observable<ValidationErrors | null> {
    if(control.hasError('email') || control.hasError('required')) return of(null);
    return this.userService.getUserEmailAvalibility(control.value).pipe(
      map((isTaken: any) => (isTaken ? null : { uniqueEmail: true })),
      catchError(() => of(null))
    );
  }
}
