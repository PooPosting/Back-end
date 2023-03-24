import { NgModule } from '@angular/core';
import {RegisterComponent} from "./register.component";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {RegisterRoutingModule} from "./register.routing.module";
import {CommonModule} from "@angular/common";
import {KeyFilterModule} from "primeng/keyfilter";
import {ChipsModule} from "primeng/chips";
import {CaptchaModule} from "primeng/captcha";
import {ButtonModule} from "primeng/button";



@NgModule({
  declarations: [
    RegisterComponent
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    RegisterRoutingModule,
    CommonModule,
    KeyFilterModule,
    ChipsModule,
    CaptchaModule,
    ButtonModule
  ]
})
export class RegisterModule { }
