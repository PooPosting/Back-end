import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {SidebarComponent} from "./sidebar.component";
import {AccordionModule} from "primeng/accordion";
import {RippleModule} from "primeng/ripple";
import {ButtonModule} from "primeng/button";
import {RouterModule} from "@angular/router";



@NgModule({
  declarations: [
    SidebarComponent
  ],
  imports: [
    CommonModule,
    AccordionModule,
    RippleModule,
    ButtonModule,
    RouterModule,

  ],
  exports: [
    SidebarComponent
  ]
})
export class SidebarModule { }
