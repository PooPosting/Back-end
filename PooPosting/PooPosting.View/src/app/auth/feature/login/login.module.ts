import { NgModule } from '@angular/core';
import {LoginComponent} from "./login.component";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {LoginRoutingModule} from "./login.routing.module";
import {CommonModule} from "@angular/common";
import {MessagesModule} from "primeng/messages";
import {InputTextModule} from "primeng/inputtext";
import {RippleModule} from "primeng/ripple";
import {ButtonModule} from "primeng/button";



@NgModule({
  declarations: [
    LoginComponent
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    LoginRoutingModule,
    CommonModule,
    MessagesModule,
    InputTextModule,
    RippleModule,
    ButtonModule
  ]
})
export class LoginModule { }
