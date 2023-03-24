import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PopularpageRoutingModule } from './popularpage-routing.module';
import {PopularComponent} from "./popular.component";
import {PictureSliderCardModule} from "../../ui/picture-slider-card/picture-slider-card.module";
import {SelectButtonModule} from "primeng/selectbutton";
import {FormsModule} from "@angular/forms";
import {CarouselModule} from "primeng/carousel";
import {TableModule} from "primeng/table";
import {ProgressSpinnerModule} from "primeng/progressspinner";


@NgModule({
  declarations: [
    PopularComponent
  ],
  imports: [
    CommonModule,
    PopularpageRoutingModule,
    PictureSliderCardModule,
    SelectButtonModule,
    FormsModule,
    CarouselModule,
    TableModule,
    ProgressSpinnerModule
  ]
})
export class PopularpageModule { }
