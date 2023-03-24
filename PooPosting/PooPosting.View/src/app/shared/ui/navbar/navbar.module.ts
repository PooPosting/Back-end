import { NgModule } from '@angular/core';
import {NavbarComponent} from "./navbar.component";
import {RouterModule} from "@angular/router";
import {CommonModule} from "@angular/common";
import {SidebarModule} from "primeng/sidebar";
import {ButtonModule} from "primeng/button";
import {SharedModule} from "primeng/api";



@NgModule({
  declarations: [
    NavbarComponent
  ],
  exports: [
    NavbarComponent
  ],
  imports: [
    RouterModule,
    CommonModule,
    SidebarModule,
    ButtonModule,
    SharedModule,

  ]
})
export class NavbarModule { }
