import { NgModule } from '@angular/core';
import {LoggedOutComponent} from "./logged-out.component";
import {LoggedOutRoutingModule} from "./logged-out.routing.module";
import {ButtonModule} from "primeng/button";
import {RippleModule} from "primeng/ripple";



@NgModule({
  declarations: [
    LoggedOutComponent,
  ],
  imports: [
    LoggedOutRoutingModule,
    ButtonModule,
    RippleModule
  ]
})
export class LoggedOutModule { }
