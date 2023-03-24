import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {SearchPanelComponent} from "./search-panel.component";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {Ripple, RippleModule} from "primeng/ripple";
import {ButtonModule} from "primeng/button";



@NgModule({
  declarations: [
    SearchPanelComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RippleModule,
    ButtonModule
  ],
  exports: [
    SearchPanelComponent
  ]
})
export class SearchPanelModule { }
