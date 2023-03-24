import {AbstractControl} from "@angular/forms";

export interface ChangePasswordModel {
  password: AbstractControl,
  confirmPassword: AbstractControl,
}
