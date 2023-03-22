import {AbstractControl} from "@angular/forms";

export interface ChangePasswordFormModel {
  password: AbstractControl,
  confirmPassword: AbstractControl,
}
