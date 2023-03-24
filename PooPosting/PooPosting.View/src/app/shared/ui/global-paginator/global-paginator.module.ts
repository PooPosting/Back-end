import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {GlobalPaginatorComponent} from "./global-paginator.component";
import {ButtonModule} from "primeng/button";
import {RippleModule} from "primeng/ripple";



@NgModule({
  declarations: [
    GlobalPaginatorComponent
  ],
  exports: [
    GlobalPaginatorComponent
  ],
  imports: [
    CommonModule,
    ButtonModule,
    RippleModule,
  ]
})
export class GlobalPaginatorModule { }
