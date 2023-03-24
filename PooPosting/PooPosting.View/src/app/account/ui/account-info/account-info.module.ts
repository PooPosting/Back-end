import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {AccountInfoComponent} from "./account-info.component";



@NgModule({
  declarations: [
    AccountInfoComponent
  ],
  exports: [
    AccountInfoComponent
  ],
  imports: [
    CommonModule
  ]
})
export class AccountInfoModule { }
