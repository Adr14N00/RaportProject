import { Directive } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, ValidationErrors, Validator, ValidatorFn } from '@angular/forms';

@Directive({
  selector: '[appRetypePassword]',
  providers: [{provide: NG_VALIDATORS, useExisting: RetypePasswordDirective, multi: true}]
})
export class RetypePasswordDirective implements Validator {

  constructor() { }

  validate(control: AbstractControl<any, any>): ValidationErrors | null {
    return retypePasswordValidator(control);
  }

}


export const retypePasswordValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
  const password = control.get('password');
  const retypedPassword = control.get('retypedPassword');
  return password?.value !== retypedPassword?.value ? { differentPassword: true } : null;
}