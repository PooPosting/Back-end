import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {HomepageComponent} from "./pages/homepage/homepage.component";
import {PopularComponent} from "./pages/popular/popular.component";
import {PictureSliderCardComponent} from "./pages/popular/picture-slider-card/picture-slider-card.component";
import {SearchComponent} from "./pages/search/search.component";
import {SharedModule} from "../Shared/shared.module";
import {PrimeNgModule} from "../Prime-ng/prime-ng.module";
import {ReactiveFormsModule} from "@angular/forms";
import {PictureModule} from "../Picture/picture.module";

@NgModule({
  declarations: [
    HomepageComponent,
    PopularComponent,
    PictureSliderCardComponent,
    SearchComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    PrimeNgModule,
    ReactiveFormsModule,
    PictureModule,
  ]
})
export class HomeModule { }
