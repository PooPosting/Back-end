import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {PictureSliderCardComponent} from "./picture-slider-card.component";
import {CardModule} from "primeng/card";
import {ButtonModule} from "primeng/button";
import {RippleModule} from "primeng/ripple";
import {RouterModule} from "@angular/router";



@NgModule({
  declarations: [
    PictureSliderCardComponent
  ],
  exports: [
    PictureSliderCardComponent
  ],
  imports: [
    CommonModule,
    CardModule,
    ButtonModule,
    RippleModule,
    RouterModule,
  ]
})
export class PictureSliderCardModule { }
